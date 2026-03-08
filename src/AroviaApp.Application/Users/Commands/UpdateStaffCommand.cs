using AroviaApp.Application.Common.Interfaces;
using AroviaApp.Domain.Entities;
using AroviaApp.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Users.Commands;

public record UpdateStaffCommand : IRequest<bool>
{
    public Guid Id { get; init; }
    public Guid ClinicId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Specialization { get; init; } = string.Empty;
    public string Qualification { get; init; } = string.Empty;
    public bool IsActive { get; init; } = true;
    public DateTime? RenewalDueDate { get; init; }
}

public class UpdateStaffCommandHandler : IRequestHandler<UpdateStaffCommand, bool>
{
    private readonly IAppDbContext _context;

    public UpdateStaffCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id && u.ClinicId == request.ClinicId, cancellationToken);

        if (user == null) return false;

        user.FullName = request.FullName;
        user.PhoneNumber = request.PhoneNumber;
        user.Specialization = request.Specialization;
        user.Qualification = request.Qualification;
        user.RenewalDueDate = request.RenewalDueDate;

        if (user.Role == UserRole.SuperAdmin)
        {
            user.IsActive = true; 
        }
        else
        {
            user.IsActive = request.IsActive;
        }

        if (Enum.TryParse<UserRole>(request.Role, out var userRole))
        {
            user.Role = userRole;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
