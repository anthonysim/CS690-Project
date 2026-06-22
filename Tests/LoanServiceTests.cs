using final_project.Models;
using final_project.Services;
using Xunit;

namespace final_project.Tests;

// Module under test: LoanService — handles checking out books and tracking overdue loans.
public class LoanServiceTests
{
    [Fact]
    public void CheckOut_AvailableBook_SucceedsAndDecrementsCopies()
    {
        var books = new List<Book> { new Book { Id = 1, Title = "Dune", Author = "Frank Herbert", Isbn = "1", TotalCopies = 1, AvailableCopies = 1 } };
        var patrons = new List<Patron> { new Patron { Id = 1, Name = "Alice" } };
        var loans = new List<Loan>();
        var service = new LoanService(books, patrons, loans);

        var (success, message) = service.CheckOut(1, 1);

        Assert.True(success);
        Assert.Equal(0, books[0].AvailableCopies);
        Assert.Single(loans);
        Assert.Contains("Alice", message);
    }

    [Fact]
    public void CheckOut_NoCopiesAvailable_Fails()
    {
        var books = new List<Book> { new Book { Id = 1, Title = "Dune", Author = "Frank Herbert", Isbn = "1", TotalCopies = 1, AvailableCopies = 0 } };
        var patrons = new List<Patron> { new Patron { Id = 1, Name = "Alice" } };
        var service = new LoanService(books, patrons, new List<Loan>());

        var (success, message) = service.CheckOut(1, 1);

        Assert.False(success);
        Assert.Contains("available", message);
    }

    [Fact]
    public void CheckOut_BlockedPatron_Fails()
    {
        var books = new List<Book> { new Book { Id = 1, Title = "Dune", Author = "Frank Herbert", Isbn = "1", TotalCopies = 1, AvailableCopies = 1 } };
        var patrons = new List<Patron> { new Patron { Id = 1, Name = "Alice", IsBlocked = true } };
        var service = new LoanService(books, patrons, new List<Loan>());

        var (success, message) = service.CheckOut(1, 1);

        Assert.False(success);
        Assert.Contains("blocked", message);
    }

    [Fact]
    public void CheckOut_PatronWithOverdueLoan_Fails()
    {
        var books = new List<Book> { new Book { Id = 1, Title = "Dune", Author = "Frank Herbert", Isbn = "1", TotalCopies = 1, AvailableCopies = 1 } };
        var patrons = new List<Patron> { new Patron { Id = 1, Name = "Alice" } };
        var loans = new List<Loan>
        {
            new Loan { Id = 1, BookId = 99, PatronId = 1, CheckoutDate = DateTime.Today.AddDays(-30), DueDate = DateTime.Today.AddDays(-16) }
        };
        var service = new LoanService(books, patrons, loans);

        var (success, message) = service.CheckOut(1, 1);

        Assert.False(success);
        Assert.Contains("overdue", message);
    }

    [Fact]
    public void GetOverdueLoans_ReturnsOnlyPastDueLoans()
    {
        var loans = new List<Loan>
        {
            new Loan { Id = 1, BookId = 1, PatronId = 1, DueDate = DateTime.Today.AddDays(-1) },
            new Loan { Id = 2, BookId = 2, PatronId = 1, DueDate = DateTime.Today.AddDays(5) }
        };
        var service = new LoanService(new List<Book>(), new List<Patron>(), loans);

        var overdue = service.GetOverdueLoans();

        Assert.Single(overdue);
        Assert.Equal(1, overdue[0].Id);
    }
}
