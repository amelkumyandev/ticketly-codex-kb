using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ticketly.Application.Abstractions;
using Ticketly.Domain.Entities;

namespace Ticketly.Infrastructure.Security;

public sealed class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    public string CreateAccessToken(ApplicationUser user)
    {
        var issuer = GetRequiredValue("Jwt:Issuer");
        var audience = GetRequiredValue("Jwt:Audience");
        var signingKey = GetRequiredValue("Jwt:SigningKey");
        var expiresMinutes = GetExpiresMinutes();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GetRequiredValue(string key)
    {
        var value = configuration[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Configuration value '{key}' is required.");
        }

        return value;
    }

    private int GetExpiresMinutes()
    {
        var configuredValue = configuration["Jwt:ExpiresMinutes"];

        return int.TryParse(configuredValue, out var expiresMinutes) && expiresMinutes > 0
            ? expiresMinutes
            : 60;
    }
}
