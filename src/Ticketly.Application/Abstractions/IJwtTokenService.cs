using Ticketly.Domain.Entities;

namespace Ticketly.Application.Abstractions;

public interface IJwtTokenService
{
    string CreateAccessToken(ApplicationUser user);
}
