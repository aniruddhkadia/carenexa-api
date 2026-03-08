using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Users.Commands;

public record DeleteStaffCommand(Guid Id, Guid ClinicId) : IRequest<bool>;

public class DeleteStaffCommandHandler : IRequestHandler<DeleteStaffCommand, bool>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteStaffCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeleteStaffCommand request, CancellationToken cancellationToken)
    {
        var isSuperAdmin = _currentUserService.Role?.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase) ?? false;
        
        // If not super admin, restrict to clinic
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id && (isSuperAdmin || u.ClinicId == request.ClinicId), cancellationToken);

        if (user == null) return false;

        // System Admin (SuperAdmin) cannot be deleted
        if (user.Role == AroviaApp.Domain.Enums.UserRole.SuperAdmin)
        {
            return false;
        }

        // Use Soft Delete to preserve foreign key integrity (Appointments, Records, Patients)
        user.IsActive = false;
        
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
