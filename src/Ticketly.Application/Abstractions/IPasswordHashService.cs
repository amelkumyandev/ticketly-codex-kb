namespace Ticketly.Application.Abstractions;

public interface IPasswordHashService
{
    string HashPassword(string email, string password);

    bool VerifyPassword(string email, string passwordHash, string password);
}
