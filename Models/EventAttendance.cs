namespace final_project.Models;

public class EventAttendance
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int PatronId { get; set; }
    public DateTime CheckInTime { get; set; }
}
