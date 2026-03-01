using CarenexaApp.Application.Common.Interfaces;
using CarenexaApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Auth.Commands;

public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<AuthResponse?>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse?>
{
    private readonly IAppDbContext _context;
    private readonly IJwtTokenService _jwtService;

    public RefreshTokenCommandHandler(IAppDbContext context, IJwtTokenService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
        var userIdStr = principal.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
        {
            return null;
        }

        var savedRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken && rt.UserId == userId, cancellationToken);

        if (savedRefreshToken == null || !savedRefreshToken.IsActive)
        {
            return null;
        }

        var user = await _context.Users.FindAsync(new object[] { userId }, cancellationToken);
        if (user == null) return null;

        var newAccessToken = _jwtService.GenerateToken(user);
        var newRefreshTokenStr = _jwtService.GenerateRefreshToken();

        // Rotate refresh token
        savedRefreshToken.IsRevoked = true;
        
        var newRefreshToken = new RefreshToken
        {
            Token = newRefreshTokenStr,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResponse(
            newAccessToken,
            newRefreshTokenStr,
            900,
            new UserDto(user.Id, user.FullName, user.Role.ToString(), user.ClinicId)
        );
    }
}
