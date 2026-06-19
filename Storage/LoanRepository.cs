using System.Globalization;
using final_project.Models;

namespace final_project.Storage;

public class LoanRepository
{
    private const string FilePath = "loans.txt";
    private const string DateFormat = "yyyy-MM-dd";

    public List<Loan> Load()
    {
        if (!File.Exists(FilePath))
            return new List<Loan>();

        var loans = new List<Loan>();
        foreach (var line in File.ReadAllLines(FilePath))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split('|');
            loans.Add(new Loan
            {
                Id = int.Parse(parts[0], CultureInfo.InvariantCulture),
                BookId = int.Parse(parts[1], CultureInfo.InvariantCulture),
                PatronId = int.Parse(parts[2], CultureInfo.InvariantCulture),
                CheckoutDate = DateTime.ParseExact(parts[3], DateFormat, CultureInfo.InvariantCulture),
                DueDate = DateTime.ParseExact(parts[4], DateFormat, CultureInfo.InvariantCulture)
            });
        }

        return loans;
    }

    public void Save(List<Loan> loans)
    {
        var lines = loans.Select(l => string.Join('|',
            l.Id, l.BookId, l.PatronId,
            l.CheckoutDate.ToString(DateFormat, CultureInfo.InvariantCulture),
            l.DueDate.ToString(DateFormat, CultureInfo.InvariantCulture)));
        File.WriteAllLines(FilePath, lines);
    }
}
