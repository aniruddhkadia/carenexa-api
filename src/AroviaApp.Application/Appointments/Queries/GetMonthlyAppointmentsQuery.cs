using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Appointments.Queries;

public record GetMonthlyAppointmentsQuery(Guid DoctorId, int Year, int Month) : IRequest<MonthlyAppointmentsResponse>;

public record MonthlyAppointmentsResponse(
    int Year,
    int Month,
    List<DailyAppointmentCount> Days
);

public record DailyAppointmentCount(
    DateTime Date,
    int Count
);

public class GetMonthlyAppointmentsQueryHandler : IRequestHandler<GetMonthlyAppointmentsQuery, MonthlyAppointmentsResponse>
{
    private readonly IAppDbContext _context;

    public GetMonthlyAppointmentsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<MonthlyAppointmentsResponse> Handle(GetMonthlyAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var startDate = new DateTime(request.Year, request.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var appointments = await _context.Appointments
            .Where(a => a.DoctorId == request.DoctorId 
                        && a.AppointmentDate >= startDate 
                        && a.AppointmentDate <= endDate)
            .GroupBy(a => a.AppointmentDate.Date)
            .Select(g => new DailyAppointmentCount(g.Key, g.Count()))
            .ToListAsync(cancellationToken);

        return new MonthlyAppointmentsResponse(request.Year, request.Month, appointments);
    }
}
