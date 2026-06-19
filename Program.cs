using final_project.Data;
using final_project.Services;
using final_project.Storage;

var bookRepository = new BookRepository();
var patronRepository = new PatronRepository();
var loanRepository = new LoanRepository();
var studyRoomRepository = new StudyRoomRepository();
var reservationRepository = new RoomReservationRepository();
var eventRepository = new EventRepository();
var attendanceRepository = new EventAttendanceRepository();

var books = bookRepository.Load();
if (books.Count == 0)
{
    books = SeedData.Books();
    bookRepository.Save(books);
}

var patrons = patronRepository.Load();
if (patrons.Count == 0)
{
    patrons = SeedData.Patrons();
    patronRepository.Save(patrons);
}

var loans = loanRepository.Load();
if (loans.Count == 0)
{
    loans = SeedData.Loans();
    loanRepository.Save(loans);
}

var rooms = studyRoomRepository.Load();
if (rooms.Count == 0)
{
    rooms = SeedData.StudyRooms();
    studyRoomRepository.Save(rooms);
}

var reservations = reservationRepository.Load();

var events = eventRepository.Load();
if (events.Count == 0)
{
    events = SeedData.Events();
    eventRepository.Save(events);
}

var attendances = attendanceRepository.Load();

var bookService = new BookService(books);
var loanService = new LoanService(books, patrons, loans);
var roomReservationService = new RoomReservationService(rooms, patrons, reservations);
var eventService = new EventService(events, patrons, attendances);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Library Branch Manager ===");
    Console.WriteLine("1. Search books");
    Console.WriteLine("2. Check out a book");
    Console.WriteLine("3. View overdue loans");
    Console.WriteLine("4. Reserve a study room");
    Console.WriteLine("5. View library events");
    Console.WriteLine("6. Check in to an event");
    Console.WriteLine("7. Exit");
    Console.Write("Choose an option: ");

    var choice = Console.ReadLine();
    Console.WriteLine();

    switch (choice)
    {
        case "1":
            SearchBooks();
            break;
        case "2":
            CheckOutBook();
            break;
        case "3":
            ViewOverdueLoans();
            break;
        case "4":
            ReserveStudyRoom();
            break;
        case "5":
            ViewLibraryEvents();
            break;
        case "6":
            CheckInToEvent();
            break;
        case "7":
            return;
        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}

void SearchBooks()
{
    Console.Write("Search by title, author, or ISBN (leave blank for all): ");
    var term = Console.ReadLine() ?? string.Empty;

    var results = bookService.Search(term).ToList();
    if (results.Count == 0)
    {
        Console.WriteLine("No books found.");
        return;
    }

    foreach (var book in results)
    {
        Console.WriteLine($"[{book.Id}] {book.Title} by {book.Author} — {book.GetStatus()} ({book.AvailableCopies} of {book.TotalCopies} available)");
    }
}

void CheckOutBook()
{
    Console.Write("Patron ID: ");
    if (!int.TryParse(Console.ReadLine(), out var patronId))
    {
        Console.WriteLine("Invalid patron ID.");
        return;
    }

    Console.Write("Book ID: ");
    if (!int.TryParse(Console.ReadLine(), out var bookId))
    {
        Console.WriteLine("Invalid book ID.");
        return;
    }

    var (success, message) = loanService.CheckOut(bookId, patronId);
    Console.WriteLine(message);

    if (success)
    {
        bookRepository.Save(books);
        loanRepository.Save(loans);
    }
}

void ViewOverdueLoans()
{
    var overdueLoans = loanService.GetOverdueLoans();
    if (overdueLoans.Count == 0)
    {
        Console.WriteLine("No overdue loans.");
        return;
    }

    foreach (var loan in overdueLoans)
    {
        var book = books.FirstOrDefault(b => b.Id == loan.BookId);
        var patron = patrons.FirstOrDefault(p => p.Id == loan.PatronId);
        var daysOverdue = (DateTime.Today - loan.DueDate).Days;
        Console.WriteLine($"{patron?.Name ?? "Unknown patron"} — \"{book?.Title ?? "Unknown book"}\" — due {loan.DueDate:MMM d, yyyy} — {daysOverdue} day(s) overdue");
    }
}

void ReserveStudyRoom()
{
    Console.WriteLine("Study rooms:");
    foreach (var room in rooms)
    {
        Console.WriteLine($"[{room.Id}] {room.Name}");
    }

    Console.Write("Patron ID: ");
    if (!int.TryParse(Console.ReadLine(), out var patronId))
    {
        Console.WriteLine("Invalid patron ID.");
        return;
    }

    Console.Write("Room ID: ");
    if (!int.TryParse(Console.ReadLine(), out var roomId))
    {
        Console.WriteLine("Invalid room ID.");
        return;
    }

    Console.Write("Date (yyyy-MM-dd): ");
    if (!DateOnly.TryParse(Console.ReadLine(), out var date))
    {
        Console.WriteLine("Invalid date.");
        return;
    }

    Console.Write("Start time (HH:mm): ");
    if (!TimeOnly.TryParse(Console.ReadLine(), out var startTime))
    {
        Console.WriteLine("Invalid start time.");
        return;
    }

    Console.Write("End time (HH:mm): ");
    if (!TimeOnly.TryParse(Console.ReadLine(), out var endTime))
    {
        Console.WriteLine("Invalid end time.");
        return;
    }

    var (success, message) = roomReservationService.Reserve(roomId, patronId, date, startTime, endTime);
    Console.WriteLine(message);

    if (success)
    {
        reservationRepository.Save(reservations);
    }
}

void ViewLibraryEvents()
{
    if (events.Count == 0)
    {
        Console.WriteLine("No upcoming events.");
        return;
    }

    foreach (var libraryEvent in events)
    {
        var attending = eventService.GetAttendanceCount(libraryEvent.Id);
        Console.WriteLine($"[{libraryEvent.Id}] {libraryEvent.Name} — {libraryEvent.Date:MMM d, yyyy} {libraryEvent.StartTime:h:mm tt}-{libraryEvent.EndTime:h:mm tt} — {attending} of {libraryEvent.Capacity} attending");
    }
}

void CheckInToEvent()
{
    Console.Write("Event ID: ");
    if (!int.TryParse(Console.ReadLine(), out var eventId))
    {
        Console.WriteLine("Invalid event ID.");
        return;
    }

    Console.Write("Patron ID: ");
    if (!int.TryParse(Console.ReadLine(), out var patronId))
    {
        Console.WriteLine("Invalid patron ID.");
        return;
    }

    var (success, message) = eventService.CheckIn(eventId, patronId);
    Console.WriteLine(message);

    if (success)
    {
        attendanceRepository.Save(attendances);
    }
}
