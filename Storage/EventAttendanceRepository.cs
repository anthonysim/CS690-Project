using System.Globalization;
using final_project.Models;

namespace final_project.Storage;

public class EventAttendanceRepository
{
    private const string FilePath = "eventattendance.txt";
    private const string DateTimeFormat = "yyyy-MM-dd HH:mm";

    public List<EventAttendance> Load()
    {
        if (!File.Exists(FilePath))
            return new List<EventAttendance>();

        var attendances = new List<EventAttendance>();
        foreach (var line in File.ReadAllLines(FilePath))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split('|');
            attendances.Add(new EventAttendance
            {
                Id = int.Parse(parts[0], CultureInfo.InvariantCulture),
                EventId = int.Parse(parts[1], CultureInfo.InvariantCulture),
                PatronId = int.Parse(parts[2], CultureInfo.InvariantCulture),
                CheckInTime = DateTime.ParseExact(parts[3], DateTimeFormat, CultureInfo.InvariantCulture)
            });
        }

        return attendances;
    }

    public void Save(List<EventAttendance> attendances)
    {
        var lines = attendances.Select(a => string.Join('|',
            a.Id, a.EventId, a.PatronId,
            a.CheckInTime.ToString(DateTimeFormat, CultureInfo.InvariantCulture)));
        File.WriteAllLines(FilePath, lines);
    }
}
