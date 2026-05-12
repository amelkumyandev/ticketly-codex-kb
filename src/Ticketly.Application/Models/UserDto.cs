namespace Ticketly.Application.Models;

public sealed record UserDto(Guid Id, string Email, string Role, DateTime CreatedAt);
