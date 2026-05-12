using Ticketly.Application.Abstractions;
using Ticketly.Application.Models;
using Ticketly.Domain.Entities;

namespace Ticketly.Application.Services;

public sealed class AuthService(
    ITicketlyRepository repository,
    IPasswordHashService passwordHashService,
    IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<ServiceResult<UserDto>> RegisterAsync(
        string? email,
        string? password,
        string? role,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return ServiceResult<UserDto>.BadRequest("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            return ServiceResult<UserDto>.BadRequest("Password is required.");
        }

        var normalizedRole = role?.Trim();

        if (!UserRoles.IsSupported(normalizedRole))
        {
            return ServiceResult<UserDto>.BadRequest("Role must be Admin or Customer.");
        }

        var normalizedEmail = email.Trim().ToLowerInvariant();

        if (await repository.UserEmailExistsAsync(normalizedEmail, cancellationToken))
        {
            return ServiceResult<UserDto>.BadRequest("A user with this email already exists.");
        }

        var passwordHash = passwordHashService.HashPassword(normalizedEmail, password);
        var user = ApplicationUser.Create(normalizedEmail, passwordHash, normalizedRole!, DateTime.UtcNow);

        await repository.AddUserAsync(user, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return ServiceResult<UserDto>.Success(ToDto(user));
    }

    public async Task<ServiceResult<AuthTokenDto>> LoginAsync(
        string? email,
        string? password,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return ServiceResult<AuthTokenDto>.Unauthorized("Invalid email or password.");
        }

        var normalizedEmail = email.Trim().ToLowerInvariant();
        var user = await repository.GetUserByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null || !passwordHashService.VerifyPassword(normalizedEmail, user.PasswordHash, password))
        {
            return ServiceResult<AuthTokenDto>.Unauthorized("Invalid email or password.");
        }

        return ServiceResult<AuthTokenDto>.Success(new AuthTokenDto(jwtTokenService.CreateAccessToken(user)));
    }

    private static UserDto ToDto(ApplicationUser user)
    {
        return new UserDto(user.Id, user.Email, user.Role, user.CreatedAt);
    }
}
