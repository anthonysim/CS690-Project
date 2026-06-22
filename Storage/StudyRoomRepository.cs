using System.Globalization;
using final_project.Models;

namespace final_project.Storage;

public class StudyRoomRepository
{
    private const string FilePath = "studyrooms.txt";

    public List<StudyRoom> Load()
    {
        if (!File.Exists(FilePath))
            return new List<StudyRoom>();

        var rooms = new List<StudyRoom>();
        foreach (var line in File.ReadAllLines(FilePath))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split('|');
            rooms.Add(new StudyRoom
            {
                Id = int.Parse(parts[0], CultureInfo.InvariantCulture),
                Name = parts[1]
            });
        }

        return rooms;
    }

    public void Save(List<StudyRoom> rooms)
    {
        var lines = rooms.Select(r => string.Join('|', r.Id, r.Name));
        File.WriteAllLines(FilePath, lines);
    }
}
