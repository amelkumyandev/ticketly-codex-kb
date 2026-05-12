namespace Ticketly.Api.Contracts;

public sealed record CreateTicketTypeRequest(string? Name, decimal Price, string? Currency, int TotalQuantity);
