using System.Globalization;
using final_project.Models;

namespace final_project.Storage;

public class RoomReservationRepository
{
    private const string FilePath = "reservations.txt";
    private const string DateFormat = "yyyy-MM-dd";
    private const string TimeFormat = "HH:mm";

    public List<RoomReservation> Load()
    {
        if (!File.Exists(FilePath))
            return new List<RoomReservation>();

        var reservations = new List<RoomReservation>();
        foreach (var line in File.ReadAllLines(FilePath))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split('|');
            reservations.Add(new RoomReservation
            {
                Id = int.Parse(parts[0], CultureInfo.InvariantCulture),
                RoomId = int.Parse(parts[1], CultureInfo.InvariantCulture),
                PatronId = int.Parse(parts[2], CultureInfo.InvariantCulture),
                Date = DateOnly.ParseExact(parts[3], DateFormat, CultureInfo.InvariantCulture),
                StartTime = TimeOnly.ParseExact(parts[4], TimeFormat, CultureInfo.InvariantCulture),
                EndTime = TimeOnly.ParseExact(parts[5], TimeFormat, CultureInfo.InvariantCulture)
            });
        }

        return reservations;
    }

    public void Save(List<RoomReservation> reservations)
    {
        var lines = reservations.Select(r => string.Join('|',
            r.Id, r.RoomId, r.PatronId,
            r.Date.ToString(DateFormat, CultureInfo.InvariantCulture),
            r.StartTime.ToString(TimeFormat, CultureInfo.InvariantCulture),
            r.EndTime.ToString(TimeFormat, CultureInfo.InvariantCulture)));
        File.WriteAllLines(FilePath, lines);
    }
}
