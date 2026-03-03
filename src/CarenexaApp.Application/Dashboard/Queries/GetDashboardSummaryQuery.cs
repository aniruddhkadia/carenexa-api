using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Dashboard.Queries;

public record GetDashboardSummaryQuery(Guid ClinicId, DateTime? Date = null) : IRequest<DashboardSummaryDto>;

public record DashboardSummaryDto(
    int TotalPatients,
    int TodaysAppointments,
    int CompletedToday,
    int PendingClaims,
    decimal MonthlyRevenue,
    List<RecentAppointmentDto> RecentAppointments
);

public record RecentAppointmentDto(
    Guid Id,
    Guid PatientId,
    string PatientName,
    DateTime AppointmentDate,
    string Status
);

public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    private readonly IAppDbContext _context;

    public GetDashboardSummaryQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        var totalPatients = await _context.Patients
            .CountAsync(p => p.ClinicId == request.ClinicId, cancellationToken);
        
        var today = request.Date?.Date ?? DateTime.UtcNow.Date;
        var todaysAppointments = await _context.Appointments
            .CountAsync(a => a.AppointmentDate.Date == today && a.Patient!.ClinicId == request.ClinicId, cancellationToken);

        var completedToday = await _context.Appointments
            .CountAsync(a => a.AppointmentDate.Date == today && a.Status == Domain.Enums.AppointmentStatus.Completed && a.Patient!.ClinicId == request.ClinicId, cancellationToken);

        var pendingClaims = await _context.InsuranceClaims
            .CountAsync(c => c.Status == Domain.Enums.ClaimStatus.Pending && c.Patient!.ClinicId == request.ClinicId, cancellationToken);

        var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var monthlyRevenue = await _context.Billings
            .Where(b => b.CreatedAt >= startOfMonth && b.Patient!.ClinicId == request.ClinicId && b.PaymentStatus == Domain.Enums.PaymentStatus.Paid)
            .SumAsync(b => b.TotalAmount, cancellationToken);

        var recentAppointments = await _context.Appointments
            .Include(a => a.Patient)
            .Where(a => a.Patient!.ClinicId == request.ClinicId && 
                        a.AppointmentDate.Date == today &&
                        a.Status != Domain.Enums.AppointmentStatus.Cancelled)
            .OrderBy(a => a.AppointmentDate)
            .Select(a => new RecentAppointmentDto(
                a.Id,
                a.PatientId,
                $"{a.Patient!.FirstName} {a.Patient.LastName}",
                a.AppointmentDate,
                a.Status.ToString()
            ))
            .ToListAsync(cancellationToken);

        return new DashboardSummaryDto(
            totalPatients,
            todaysAppointments,
            completedToday,
            pendingClaims,
            monthlyRevenue,
            recentAppointments
        );
    }
}
