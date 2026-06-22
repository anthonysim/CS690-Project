using final_project.Models;
using final_project.Services;
using Xunit;

namespace final_project.Tests;

// Module under test: RoomReservationService — books study rooms and prevents double-booking.
public class RoomReservationServiceTests
{
    private static (List<StudyRoom> rooms, List<Patron> patrons) SampleData() =>
        (
            new List<StudyRoom> { new StudyRoom { Id = 1, Name = "Room A" } },
            new List<Patron> { new Patron { Id = 1, Name = "Alice" } }
        );

    [Fact]
    public void Reserve_OpenSlot_Succeeds()
    {
        var (rooms, patrons) = SampleData();
        var service = new RoomReservationService(rooms, patrons, new List<RoomReservation>());

        var (success, message) = service.Reserve(1, 1, new DateOnly(2026, 6, 22), new TimeOnly(10, 0), new TimeOnly(11, 0));

        Assert.True(success);
        Assert.Contains("Room A", message);
    }

    [Fact]
    public void Reserve_EndTimeBeforeStartTime_Fails()
    {
        var (rooms, patrons) = SampleData();
        var service = new RoomReservationService(rooms, patrons, new List<RoomReservation>());

        var (success, message) = service.Reserve(1, 1, new DateOnly(2026, 6, 22), new TimeOnly(11, 0), new TimeOnly(10, 0));

        Assert.False(success);
        Assert.Equal("End time must be after start time.", message);
    }

    [Fact]
    public void Reserve_OverlappingTimeSlot_Fails()
    {
        var (rooms, patrons) = SampleData();
        var existing = new RoomReservation { Id = 1, RoomId = 1, PatronId = 1, Date = new DateOnly(2026, 6, 22), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 0) };
        var service = new RoomReservationService(rooms, patrons, new List<RoomReservation> { existing });

        var (success, message) = service.Reserve(1, 1, new DateOnly(2026, 6, 22), new TimeOnly(10, 30), new TimeOnly(11, 30));

        Assert.False(success);
        Assert.Contains("already booked", message);
    }

    [Fact]
    public void Reserve_SameRoomDifferentDay_Succeeds()
    {
        var (rooms, patrons) = SampleData();
        var existing = new RoomReservation { Id = 1, RoomId = 1, PatronId = 1, Date = new DateOnly(2026, 6, 22), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 0) };
        var service = new RoomReservationService(rooms, patrons, new List<RoomReservation> { existing });

        var (success, _) = service.Reserve(1, 1, new DateOnly(2026, 6, 23), new TimeOnly(10, 0), new TimeOnly(11, 0));

        Assert.True(success);
    }

    [Fact]
    public void Reserve_UnknownRoom_Fails()
    {
        var (rooms, patrons) = SampleData();
        var service = new RoomReservationService(rooms, patrons, new List<RoomReservation>());

        var (success, message) = service.Reserve(99, 1, new DateOnly(2026, 6, 22), new TimeOnly(10, 0), new TimeOnly(11, 0));

        Assert.False(success);
        Assert.Equal("Room not found.", message);
    }
}
