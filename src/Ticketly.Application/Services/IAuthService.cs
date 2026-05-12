using Ticketly.Application.Models;

namespace Ticketly.Application.Services;

public interface IAuthService
{
    Task<ServiceResult<UserDto>> RegisterAsync(
        string? email,
        string? password,
        string? role,
        CancellationToken cancellationToken);

    Task<ServiceResult<AuthTokenDto>> LoginAsync(
        string? email,
        string? password,
        CancellationToken cancellationToken);
}
