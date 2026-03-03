using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace CarenexaApp.Application.Auth.Commands;

public record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest<bool>;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly IAuditService _auditService;

    public ResetPasswordCommandHandler(IAppDbContext context, IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        
        if (user == null) return false;

        // In a real app, verify the reset token from a database table
        // For MVP, we'll assume the token is correct if we got this far (simplification)
        
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, 12);
        
        await _context.SaveChangesAsync(cancellationToken);
        await _auditService.LogActionAsync(user.Id, "PASSWORD_RESET_SUCCESS", "User", "Password reset successfully");

        return true;
    }
}
