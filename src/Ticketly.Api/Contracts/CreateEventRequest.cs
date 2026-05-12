namespace Ticketly.Api.Contracts;

public sealed record CreateEventRequest(string? Name, string? Venue, DateTime StartsAt);
