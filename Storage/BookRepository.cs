using System.Globalization;
using final_project.Models;

namespace final_project.Storage;

public class BookRepository
{
    private const string FilePath = "books.txt";

    public List<Book> Load()
    {
        if (!File.Exists(FilePath))
            return new List<Book>();

        var books = new List<Book>();
        foreach (var line in File.ReadAllLines(FilePath))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split('|');
            books.Add(new Book
            {
                Id = int.Parse(parts[0], CultureInfo.InvariantCulture),
                Title = parts[1],
                Author = parts[2],
                Isbn = parts[3],
                TotalCopies = int.Parse(parts[4], CultureInfo.InvariantCulture),
                AvailableCopies = int.Parse(parts[5], CultureInfo.InvariantCulture)
            });
        }

        return books;
    }

    public void Save(List<Book> books)
    {
        var lines = books.Select(b => string.Join('|', b.Id, b.Title, b.Author, b.Isbn, b.TotalCopies, b.AvailableCopies));
        File.WriteAllLines(FilePath, lines);
    }
}
