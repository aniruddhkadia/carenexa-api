using AroviaApp.Application.Common.Interfaces;
using AroviaApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace AroviaApp.Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;

public record AuthResponse(string AccessToken, string RefreshToken, int ExpiresIn, UserDto User);

public record UserDto(Guid Id, string FullName, string Role, Guid ClinicId);

public record LoginResult
{
    public bool Success { get; init; }
    public AuthResponse? Response { get; init; }
    public string? ErrorMessage { get; init; }
    public bool IsInactive { get; init; }

    public static LoginResult Succeeded(AuthResponse response) => new() { Success = true, Response = response };
    public static LoginResult Failed(string message, bool isInactive = false) => new() { Success = false, ErrorMessage = message, IsInactive = isInactive };
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
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

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        // Verify password using BCrypt
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            await _auditService.LogActionAsync(null, "LOGIN_FAILED", "User", $"Attempted login for email: {request.Email}");
            return LoginResult.Failed("Invalid email or password");
        }

        // Check for renewal due date expiration
        if (user.RenewalDueDate.HasValue && user.RenewalDueDate.Value.Date < DateTime.UtcNow.Date)
        {
            if (user.IsActive)
            {
                user.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
            
            await _auditService.LogActionAsync(user.Id, "LOGIN_FAILED_EXPIRED", "User", "Account disabled because renewal due date passed");
            return LoginResult.Failed("Your account has been deactivated. Please contact the administrator.", true);
        }

        if (!user.IsActive)
        {
            await _auditService.LogActionAsync(user.Id, "LOGIN_FAILED_INACTIVE", "User", "Inactive user attempt");
            return LoginResult.Failed("Your account has been deactivated. Please contact the administrator.", true);
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

        return LoginResult.Succeeded(new AuthResponse(
            accessToken,
            refreshTokenStr,
            900, // 15 minutes
            new UserDto(user.Id, user.FullName, user.Role.ToString(), user.ClinicId)
        ));
    }
}
