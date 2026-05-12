namespace Ticketly.Domain.Entities;

public sealed class TicketType
{
    private TicketType()
    {
        Name = string.Empty;
        Currency = string.Empty;
    }

    private TicketType(
        Guid id,
        Guid eventId,
        string name,
        decimal price,
        string currency,
        int totalQuantity,
        DateTime createdAt)
    {
        Id = id;
        EventId = eventId;
        Name = name;
        Price = price;
        Currency = currency;
        TotalQuantity = totalQuantity;
        AvailableQuantity = totalQuantity;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }

    public Guid EventId { get; private set; }

    public string Name { get; private set; }

    public decimal Price { get; private set; }

    public string Currency { get; private set; }

    public int TotalQuantity { get; private set; }

    public int AvailableQuantity { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static TicketType Create(
        Guid eventId,
        string name,
        decimal price,
        string currency,
        int totalQuantity,
        DateTime createdAt)
    {
        if (eventId == Guid.Empty)
        {
            throw new ArgumentException("Event id is required.", nameof(eventId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Ticket type name is required.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Ticket type price cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Ticket type currency is required.", nameof(currency));
        }

        if (totalQuantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalQuantity), "Ticket type total quantity must be greater than zero.");
        }

        return new TicketType(
            Guid.NewGuid(),
            eventId,
            name.Trim(),
            price,
            currency.Trim().ToUpperInvariant(),
            totalQuantity,
            ToUtc(createdAt));
    }

    public void Reserve(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Reservation quantity must be greater than zero.");
        }

        if (quantity > AvailableQuantity)
        {
            throw new InvalidOperationException("Not enough tickets available.");
        }

        AvailableQuantity -= quantity;
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
