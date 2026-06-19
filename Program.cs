using final_project.Data;
using final_project.Models;
using final_project.Services;

var books = SeedData.Books();
var patrons = SeedData.Patrons();
var loans = new List<Loan>();

var bookService = new BookService(books);
var loanService = new LoanService(books, patrons, loans);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Library Branch Manager ===");
    Console.WriteLine("1. Search books");
    Console.WriteLine("2. Check out a book");
    Console.WriteLine("3. Exit");
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
}
