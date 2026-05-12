namespace Ticketly.Api.Contracts;

public sealed record LoginRequest(string? Email, string? Password);
