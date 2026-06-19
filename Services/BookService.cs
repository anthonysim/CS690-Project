using final_project.Models;

namespace final_project.Services;

public class BookService
{
    private readonly List<Book> _books;

    public BookService(List<Book> books)
    {
        _books = books;
    }

    public IEnumerable<Book> Search(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return _books;

        return _books.Where(b =>
            b.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(term, StringComparison.OrdinalIgnoreCase) ||
            b.Isbn.Contains(term, StringComparison.OrdinalIgnoreCase));
    }
}
