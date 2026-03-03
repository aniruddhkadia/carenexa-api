using CarenexaApp.Application.Common.Interfaces;
using CarenexaApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace CarenexaApp.Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponse?>;

public record AuthResponse(string AccessToken, string RefreshToken, int ExpiresIn, UserDto User);

public record UserDto(Guid Id, string FullName, string Role, Guid ClinicId);

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse?>
{
    private readonly IAppDbContext _context;
    private readonly IJwtTokenService _jwtService;
    private readonly IAuditService _auditService;

    public LoginCommandHandler(IAppDbContext context, IJwtTokenService jwtService, IAuditService auditService)
    {
        _context = context;
        _jwtService = jwtService;
        _auditService = auditService;
    }

    public async Task<AuthResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        // Verify password using BCrypt
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            await _auditService.LogActionAsync(null, "LOGIN_FAILED", "User", $"Attempted login for email: {request.Email}");
            return null;
        }

        if (!user.IsActive)
        {
            await _auditService.LogActionAsync(user.Id, "LOGIN_FAILED_INACTIVE", "User", "Inactive user attempt");
            return null;
        }

        var accessToken = _jwtService.GenerateToken(user);
        var refreshTokenStr = _jwtService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenStr,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7), // Should come from config
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync(cancellationToken);
        await _auditService.LogActionAsync(user.Id, "LOGIN_SUCCESS", "User", "User logged in successfully");

        return new AuthResponse(
            accessToken,
            refreshTokenStr,
            900, // 15 minutes
            new UserDto(user.Id, user.FullName, user.Role.ToString(), user.ClinicId)
        );
    }
}
