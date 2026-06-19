using final_project.Models;

namespace final_project.Services;

public class EventService
{
    private readonly List<Event> _events;
    private readonly List<Patron> _patrons;
    private readonly List<EventAttendance> _attendances;
    private int _nextAttendanceId;

    public EventService(List<Event> events, List<Patron> patrons, List<EventAttendance> attendances)
    {
        _events = events;
        _patrons = patrons;
        _attendances = attendances;
        _nextAttendanceId = attendances.Count == 0 ? 1 : attendances.Max(a => a.Id) + 1;
    }

    public int GetAttendanceCount(int eventId) =>
        _attendances.Count(a => a.EventId == eventId);

    public (bool Success, string Message) CheckIn(int eventId, int patronId)
    {
        var @event = _events.FirstOrDefault(e => e.Id == eventId);
        if (@event is null)
            return (false, "Event not found.");

        var patron = _patrons.FirstOrDefault(p => p.Id == patronId);
        if (patron is null)
            return (false, "Patron not found.");

        if (GetAttendanceCount(eventId) >= @event.Capacity)
            return (false, $"{@event.Name} is at capacity ({@event.Capacity}). Check-in declined.");

        _attendances.Add(new EventAttendance
        {
            Id = _nextAttendanceId++,
            EventId = eventId,
            PatronId = patronId,
            CheckInTime = DateTime.Now
        });

        return (true, $"{patron.Name} checked in to {@event.Name}.");
    }
}
