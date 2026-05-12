namespace Ticketly.Domain.Entities;

public sealed class Reservation
{
    private Reservation()
    {
        CustomerEmail = string.Empty;
    }

    private Reservation(Guid id, Guid ticketTypeId, int quantity, string customerEmail, DateTime createdAt)
    {
        Id = id;
        TicketTypeId = ticketTypeId;
        Quantity = quantity;
        CustomerEmail = customerEmail;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }

    public Guid TicketTypeId { get; private set; }

    public int Quantity { get; private set; }

    public string CustomerEmail { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static Reservation Create(Guid ticketTypeId, int quantity, string customerEmail, DateTime createdAt)
    {
        if (ticketTypeId == Guid.Empty)
        {
            throw new ArgumentException("Ticket type id is required.", nameof(ticketTypeId));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Reservation quantity must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(customerEmail))
        {
            throw new ArgumentException("Customer email is required.", nameof(customerEmail));
        }

        return new Reservation(Guid.NewGuid(), ticketTypeId, quantity, customerEmail.Trim(), ToUtc(createdAt));
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
