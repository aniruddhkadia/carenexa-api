using AroviaApp.Domain.Entities;

namespace AroviaApp.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
    System.Security.Claims.ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
