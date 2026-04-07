public sealed class InMemoryCalendarStore : ICalendarStore
{
    private readonly List<CalendarEvent> _events =
    [
        new CalendarEvent
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Workshop: Agent-to-Agent (A2A) with Microsoft Agent Framework",
            Start = new DateTime(2026, 4, 21, 18, 0, 0),
            End = new DateTime(2026, 4, 21, 22, 0, 0),
            Location = "BCIT Downtown Campus, Room 645"
        },
        new CalendarEvent
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Workshop: Agent-to-Agent (A2A) with Microsoft Agent Framework",
            Start = new DateTime(2026, 4, 21, 12, 0, 0),
            End = new DateTime(2026, 4, 21, 13, 0, 0),
            Location = "BCIT Downtown Campus, Room 645"
        },
        new CalendarEvent
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Workshop: Agent-to-Agent (A2A) with Microsoft Agent Framework",
            Start = new DateTime(2026, 4, 21, 13, 0, 0),
            End = new DateTime(2026, 4, 21, 15, 0, 0),
            Location = "BCIT Downtown Campus, Room 645"
        },
        new CalendarEvent
        {
            Id = Guid.NewGuid().ToString(),
            Title = "1:1 with Manager",
            Start = new DateTime(2026, 4, 22, 11, 0, 0),
            End = new DateTime(2026, 4, 22, 11, 30, 0),
            Location = "Room 201"
        }
    ];

    public IReadOnlyList<CalendarEvent> GetEvents(DateOnly date)
    {
        return _events
            .Where(e => DateOnly.FromDateTime(e.Start) == date)
            .ToList();
    }

    public void AddEvent(CalendarEvent calendarEvent)
    {
        _events.Add(calendarEvent);
    }
}