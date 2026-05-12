namespace Ticketly.Domain.Entities;

public sealed class ApplicationUser
{
    private ApplicationUser()
    {
        Email = string.Empty;
        PasswordHash = string.Empty;
        Role = string.Empty;
    }

    private ApplicationUser(Guid id, string email, string passwordHash, string role, DateTime createdAt)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }

    public string Email { get; private set; }

    public string PasswordHash { get; private set; }

    public string Role { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static ApplicationUser Create(string email, string passwordHash, string role, DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));
        }

        if (!UserRoles.IsSupported(role))
        {
            throw new ArgumentException("Role must be Admin or Customer.", nameof(role));
        }

        return new ApplicationUser(
            Guid.NewGuid(),
            email.Trim().ToLowerInvariant(),
            passwordHash,
            role.Trim(),
            ToUtc(createdAt));
    }

    private static DateTime ToUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }
}
