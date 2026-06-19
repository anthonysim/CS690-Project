using final_project.Models;

namespace final_project.Services;

public class LoanService
{
    private readonly List<Book> _books;
    private readonly List<Patron> _patrons;
    private readonly List<Loan> _loans;
    private int _nextLoanId = 1;

    public LoanService(List<Book> books, List<Patron> patrons, List<Loan> loans)
    {
        _books = books;
        _patrons = patrons;
        _loans = loans;
    }

    public (bool Success, string Message) CheckOut(int bookId, int patronId)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);
        if (book is null)
            return (false, "Book not found.");

        var patron = _patrons.FirstOrDefault(p => p.Id == patronId);
        if (patron is null)
            return (false, "Patron not found.");

        if (book.AvailableCopies <= 0)
            return (false, $"No copies of \"{book.Title}\" are available.");

        book.AvailableCopies--;

        var dueDate = DateTime.Today.AddDays(14);
        _loans.Add(new Loan
        {
            Id = _nextLoanId++,
            BookId = book.Id,
            PatronId = patron.Id,
            CheckoutDate = DateTime.Today,
            DueDate = dueDate
        });

        return (true, $"\"{book.Title}\" checked out to {patron.Name}. Due {dueDate:MMM d, yyyy}.");
    }
}
