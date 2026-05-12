using Microsoft.AspNetCore.Identity;
using Ticketly.Application.Abstractions;

namespace Ticketly.Infrastructure.Security;

public sealed class IdentityPasswordHashService : IPasswordHashService
{
    private readonly PasswordHasher<string> passwordHasher = new();

    public string HashPassword(string email, string password)
    {
        return passwordHasher.HashPassword(email, password);
    }

    public bool VerifyPassword(string email, string passwordHash, string password)
    {
        var result = passwordHasher.VerifyHashedPassword(email, passwordHash, password);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
