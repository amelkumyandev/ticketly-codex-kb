namespace Ticketly.Application.Models;

public sealed record EventDto(Guid Id, string Name, string Venue, DateTime StartsAt, DateTime CreatedAt);
