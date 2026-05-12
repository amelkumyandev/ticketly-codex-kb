namespace Ticketly.Application.Models;

public sealed record TicketTypeDto(
    Guid Id,
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    int TotalQuantity,
    int AvailableQuantity,
    DateTime CreatedAt);
