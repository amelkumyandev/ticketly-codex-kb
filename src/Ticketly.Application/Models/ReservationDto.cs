namespace Ticketly.Application.Models;

public sealed record ReservationDto(Guid Id, Guid TicketTypeId, int Quantity, string CustomerEmail, DateTime CreatedAt);
