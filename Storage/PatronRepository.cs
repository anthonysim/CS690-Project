using System.Globalization;
using final_project.Models;

namespace final_project.Storage;

public class PatronRepository
{
    private const string FilePath = "patrons.txt";

    public List<Patron> Load()
    {
        if (!File.Exists(FilePath))
            return new List<Patron>();

        var patrons = new List<Patron>();
        foreach (var line in File.ReadAllLines(FilePath))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split('|');
            patrons.Add(new Patron
            {
                Id = int.Parse(parts[0], CultureInfo.InvariantCulture),
                Name = parts[1]
            });
        }

        return patrons;
    }

    public void Save(List<Patron> patrons)
    {
        var lines = patrons.Select(p => string.Join('|', p.Id, p.Name));
        File.WriteAllLines(FilePath, lines);
    }
}
