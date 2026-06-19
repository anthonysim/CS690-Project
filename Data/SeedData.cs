using final_project.Models;

namespace final_project.Data;

public static class SeedData
{
    public static List<Book> Books() => new()
    {
        new Book { Id = 1, Title = "The Midnight Library", Author = "Matt Haig", Isbn = "9780525559474", TotalCopies = 1, AvailableCopies = 0 },
        new Book { Id = 2, Title = "Atomic Habits", Author = "James Clear", Isbn = "9780735211292", TotalCopies = 3, AvailableCopies = 2 },
        new Book { Id = 3, Title = "Where the Crawdads Sing", Author = "Delia Owens", Isbn = "9780735219090", TotalCopies = 2, AvailableCopies = 1 },
    };

    public static List<Patron> Patrons() => new()
    {
        new Patron { Id = 1, Name = "Maria Lopez" },
        new Patron { Id = 2, Name = "James Carter" },
        new Patron { Id = 3, Name = "Sam Reed", IsBlocked = true },
    };

    public static List<Loan> Loans() => new()
    {
        new Loan { Id = 1, BookId = 3, PatronId = 1, CheckoutDate = DateTime.Today.AddDays(-20), DueDate = DateTime.Today.AddDays(-6) },
    };

    public static List<StudyRoom> StudyRooms() => new()
    {
        new StudyRoom { Id = 1, Name = "Room A" },
        new StudyRoom { Id = 2, Name = "Room B" },
    };

    public static List<Event> Events() => new()
    {
        new Event { Id = 1, Name = "Children's Story Hour", Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 0), Capacity = 2 },
        new Event { Id = 2, Name = "Author Talk: Local Writers", Date = DateOnly.FromDateTime(DateTime.Today.AddDays(3)), StartTime = new TimeOnly(18, 0), EndTime = new TimeOnly(19, 30), Capacity = 20 },
    };
}
