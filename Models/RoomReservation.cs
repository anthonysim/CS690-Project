namespace final_project.Models;

public class RoomReservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int PatronId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
