using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Dashboard.Queries;

public record GetRecentActivityQuery(Guid ClinicId) : IRequest<List<ActivityDto>>;

public record ActivityDto(
    string Message,
    DateTime CreatedAt,
    string Action,
    string Entity
);

public class GetRecentActivityQueryHandler : IRequestHandler<GetRecentActivityQuery, List<ActivityDto>>
{
    private readonly IAppDbContext _context;

    public GetRecentActivityQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ActivityDto>> Handle(GetRecentActivityQuery request, CancellationToken cancellationToken)
    {
        var logs = await _context.AuditLogs
            .Include(l => l.User)
            .Where(l => l.User!.ClinicId == request.ClinicId)
            .OrderByDescending(l => l.CreatedAt)
            .Take(10)
            .Select(l => new ActivityDto(
                $"{l.Action} {l.Entity}", // Simple mapping for now, can be improved
                l.CreatedAt,
                l.Action,
                l.Entity
            ))
            .ToListAsync(cancellationToken);

        return logs;
    }
}
