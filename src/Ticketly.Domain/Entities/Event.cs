namespace Ticketly.Domain.Entities;

public sealed class Event
{
    private Event()
    {
        Name = string.Empty;
        Venue = string.Empty;
    }

    private Event(Guid id, string name, string venue, DateTime startsAt, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Venue = venue;
        StartsAt = startsAt;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Venue { get; private set; }

    public DateTime StartsAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static Event Create(string name, string venue, DateTime startsAt, DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Event name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(venue))
        {
            throw new ArgumentException("Event venue is required.", nameof(venue));
        }

        return new Event(Guid.NewGuid(), name.Trim(), venue.Trim(), ToUtc(startsAt), ToUtc(createdAt));
    }

    private static DateTime ToUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }
}
