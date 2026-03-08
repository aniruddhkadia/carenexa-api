using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AroviaApp.Domain.Enums;

namespace AroviaApp.Application.Dashboard.Queries;

public record GetDailyCompletedVisitsQuery(Guid ClinicId, DateTime Date) : IRequest<List<DailyVisitDto>>;

public record DailyVisitDto(
    Guid Id,
    Guid PatientId,
    string PatientName,
    DateTime CompletedAt,
    string Diagnosis,
    string DoctorName
);

public class GetDailyCompletedVisitsQueryHandler : IRequestHandler<GetDailyCompletedVisitsQuery, List<DailyVisitDto>>
{
    private readonly IAppDbContext _context;

    public GetDailyCompletedVisitsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DailyVisitDto>> Handle(GetDailyCompletedVisitsQuery request, CancellationToken cancellationToken)
    {
        var targetDate = request.Date.Date;
        
        return await _context.MedicalRecords
            .Include(m => m.Patient)
            .Include(m => m.Doctor)
            .Where(m => m.ClinicId == request.ClinicId && 
                        m.Status == MedicalRecordStatus.Completed && 
                        m.CreatedAt.Date == targetDate)
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new DailyVisitDto(
                m.Id,
                m.PatientId,
                $"{m.Patient!.FirstName} {m.Patient.LastName}",
                m.CreatedAt,
                m.Diagnosis,
                m.Doctor!.FullName
            ))
            .ToListAsync(cancellationToken);
    }
}
