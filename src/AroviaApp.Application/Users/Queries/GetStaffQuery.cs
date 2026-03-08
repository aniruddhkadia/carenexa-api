using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Users.Queries;

public record GetStaffQuery(Guid ClinicId) : IRequest<List<UserProfileDto>>;

public class GetStaffQueryHandler : IRequestHandler<GetStaffQuery, List<UserProfileDto>>
{
    private readonly IAppDbContext _context;

    public GetStaffQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserProfileDto>> Handle(GetStaffQuery request, CancellationToken cancellationToken)
    {
        var todayDate = DateTime.UtcNow.Date;

        return await _context.Users
            .AsNoTracking()
            .Where(x => x.ClinicId == request.ClinicId)
            .OrderBy(x => x.FullName)
            .Select(user => new UserProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Specialization = user.Specialization,
                Qualification = user.Qualification,
                Role = user.Role.ToString(),
                IsActive = user.IsActive && (!user.RenewalDueDate.HasValue || user.RenewalDueDate.Value.Date >= todayDate),
                CreatedAt = user.CreatedAt,
                RenewalDueDate = user.RenewalDueDate
            })
            .ToListAsync(cancellationToken);
    }
}
