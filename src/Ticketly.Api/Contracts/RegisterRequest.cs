namespace Ticketly.Api.Contracts;

public sealed record RegisterRequest(string? Email, string? Password, string? Role);
