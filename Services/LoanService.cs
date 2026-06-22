using final_project.Models;

namespace final_project.Services;

public class LoanService
{
    private readonly List<Book> _books;
    private readonly List<Patron> _patrons;
    private readonly List<Loan> _loans;
    private int _nextLoanId;

    public LoanService(List<Book> books, List<Patron> patrons, List<Loan> loans)
    {
        _books = books;
        _patrons = patrons;
        _loans = loans;
        _nextLoanId = loans.Count == 0 ? 1 : loans.Max(l => l.Id) + 1;
    }

    public (bool Success, string Message) CheckOut(int bookId, int patronId)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);
        if (book is null)
            return (false, "Book not found.");

        var patron = _patrons.FirstOrDefault(p => p.Id == patronId);
        if (patron is null)
            return (false, "Patron not found.");

        if (patron.IsBlocked)
            return (false, $"{patron.Name}'s account is blocked and cannot borrow books.");

        if (HasOverdueLoans(patronId))
            return (false, $"{patron.Name} has overdue items and cannot borrow more books until they are resolved.");

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

    public bool HasOverdueLoans(int patronId) =>
        _loans.Any(l => l.PatronId == patronId && l.DueDate < DateTime.Today);

    public List<Loan> GetOverdueLoans() =>
        _loans.Where(l => l.DueDate < DateTime.Today).ToList();
}
