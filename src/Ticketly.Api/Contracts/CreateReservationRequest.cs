namespace Ticketly.Api.Contracts;

public sealed record CreateReservationRequest(Guid TicketTypeId, int Quantity, string? CustomerEmail);
