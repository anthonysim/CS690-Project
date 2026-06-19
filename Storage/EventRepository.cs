using System.Globalization;
using final_project.Models;

namespace final_project.Storage;

public class EventRepository
{
    private const string FilePath = "events.txt";
    private const string DateFormat = "yyyy-MM-dd";
    private const string TimeFormat = "HH:mm";

    public List<Event> Load()
    {
        if (!File.Exists(FilePath))
            return new List<Event>();

        var events = new List<Event>();
        foreach (var line in File.ReadAllLines(FilePath))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split('|');
            events.Add(new Event
            {
                Id = int.Parse(parts[0], CultureInfo.InvariantCulture),
                Name = parts[1],
                Date = DateOnly.ParseExact(parts[2], DateFormat, CultureInfo.InvariantCulture),
                StartTime = TimeOnly.ParseExact(parts[3], TimeFormat, CultureInfo.InvariantCulture),
                EndTime = TimeOnly.ParseExact(parts[4], TimeFormat, CultureInfo.InvariantCulture),
                Capacity = int.Parse(parts[5], CultureInfo.InvariantCulture)
            });
        }

        return events;
    }

    public void Save(List<Event> events)
    {
        var lines = events.Select(e => string.Join('|',
            e.Id, e.Name,
            e.Date.ToString(DateFormat, CultureInfo.InvariantCulture),
            e.StartTime.ToString(TimeFormat, CultureInfo.InvariantCulture),
            e.EndTime.ToString(TimeFormat, CultureInfo.InvariantCulture),
            e.Capacity));
        File.WriteAllLines(FilePath, lines);
    }
}
