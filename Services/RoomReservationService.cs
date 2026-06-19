using final_project.Models;

namespace final_project.Services;

public class RoomReservationService
{
    private readonly List<StudyRoom> _rooms;
    private readonly List<Patron> _patrons;
    private readonly List<RoomReservation> _reservations;
    private int _nextReservationId;

    public RoomReservationService(List<StudyRoom> rooms, List<Patron> patrons, List<RoomReservation> reservations)
    {
        _rooms = rooms;
        _patrons = patrons;
        _reservations = reservations;
        _nextReservationId = reservations.Count == 0 ? 1 : reservations.Max(r => r.Id) + 1;
    }

    public (bool Success, string Message) Reserve(int roomId, int patronId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        var room = _rooms.FirstOrDefault(r => r.Id == roomId);
        if (room is null)
            return (false, "Room not found.");

        var patron = _patrons.FirstOrDefault(p => p.Id == patronId);
        if (patron is null)
            return (false, "Patron not found.");

        if (endTime <= startTime)
            return (false, "End time must be after start time.");

        var conflict = _reservations.Any(r =>
            r.RoomId == roomId &&
            r.Date == date &&
            startTime < r.EndTime && r.StartTime < endTime);

        if (conflict)
            return (false, $"{room.Name} is already booked for part of that time on {date:MMM d, yyyy}.");

        _reservations.Add(new RoomReservation
        {
            Id = _nextReservationId++,
            RoomId = roomId,
            PatronId = patronId,
            Date = date,
            StartTime = startTime,
            EndTime = endTime
        });

        return (true, $"{room.Name} reserved for {patron.Name} on {date:MMM d, yyyy} from {startTime:h:mm tt} to {endTime:h:mm tt}.");
    }
}
