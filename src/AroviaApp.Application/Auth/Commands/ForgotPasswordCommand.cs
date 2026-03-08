using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Auth.Commands;

public record ForgotPasswordCommand(string Email) : IRequest<bool>;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly IAuditService _auditService;
    // private readonly IEmailService _emailService; // Assuming this exists or will be added

    public ForgotPasswordCommandHandler(IAppDbContext context, IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        
        // Security best practice: Always return true (or success) to prevent account enumeration
        if (user == null)
        {
            await _auditService.LogActionAsync(null, "FORGOT_PASSWORD_REQUEST", "User", $"Request for non-existent email: {request.Email}");
            return true;
        }

        // Generate a reset token (simple implementation for MVP)
        // In a real app, you'd store this in a PasswordResets table with an expiry
        var resetToken = Guid.NewGuid().ToString();
        
        await _auditService.LogActionAsync(user.Id, "FORGOT_PASSWORD_REQUEST", "User", "Password reset requested");
        
        // TODO: Send email with resetToken
        // await _emailService.SendResetPasswordEmail(user.Email, resetToken);

        return true;
    }
}
