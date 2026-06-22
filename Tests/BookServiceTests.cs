using final_project.Models;
using final_project.Services;
using Xunit;

namespace final_project.Tests;

// Module under test: BookService — searches the catalog by title, author, or ISBN.
public class BookServiceTests
{
    private static List<Book> SampleBooks() => new()
    {
        new Book { Id = 1, Title = "The Hobbit", Author = "J.R.R. Tolkien", Isbn = "111", TotalCopies = 2, AvailableCopies = 1 },
        new Book { Id = 2, Title = "Dune", Author = "Frank Herbert", Isbn = "222", TotalCopies = 1, AvailableCopies = 0 },
    };

    [Fact]
    public void Search_BlankTerm_ReturnsAllBooks()
    {
        var service = new BookService(SampleBooks());

        var results = service.Search("");

        Assert.Equal(2, results.Count());
    }

    [Fact]
    public void Search_MatchesTitleCaseInsensitive()
    {
        var service = new BookService(SampleBooks());

        var results = service.Search("hobbit");

        Assert.Single(results);
        Assert.Equal("The Hobbit", results.First().Title);
    }

    [Fact]
    public void Search_MatchesAuthorOrIsbn()
    {
        var service = new BookService(SampleBooks());

        var byAuthor = service.Search("Herbert");
        var byIsbn = service.Search("222");

        Assert.Single(byAuthor);
        Assert.Single(byIsbn);
        Assert.Equal("Dune", byAuthor.First().Title);
    }

    [Fact]
    public void Search_NoMatch_ReturnsEmpty()
    {
        var service = new BookService(SampleBooks());

        var results = service.Search("nonexistent");

        Assert.Empty(results);
    }
}
