using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Appointments.Queries;

public record GetAppointmentsQuery(Guid DoctorId, DateTime? Date) : IRequest<List<AppointmentDto>>;

public record AppointmentDto(
    Guid Id,
    Guid PatientId,
    string PatientName,
    DateTime AppointmentDate,
    string Status,
    string Notes
);

public class GetAppointmentsQueryHandler : IRequestHandler<GetAppointmentsQuery, List<AppointmentDto>>
{
    private readonly IAppDbContext _context;

    public GetAppointmentsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AppointmentDto>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Appointments
            .Include(a => a.Patient)
            .Where(a => a.DoctorId == request.DoctorId);

        if (request.Date.HasValue)
        {
            var date = request.Date.Value.Date;
            query = query.Where(a => a.AppointmentDate.Date == date);
        }

        return await query
            .Select(a => new AppointmentDto(
                a.Id,
                a.PatientId,
                $"{a.Patient!.FirstName} {a.Patient.LastName}",
                a.AppointmentDate,
                a.Status.ToString(),
                a.Notes
            ))
            .ToListAsync(cancellationToken);
    }
}
