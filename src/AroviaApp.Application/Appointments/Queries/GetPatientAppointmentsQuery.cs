using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Appointments.Queries;

public record GetPatientAppointmentsQuery(Guid PatientId) : IRequest<List<AppointmentDto>>;

public class GetPatientAppointmentsQueryHandler : IRequestHandler<GetPatientAppointmentsQuery, List<AppointmentDto>>
{
    private readonly IAppDbContext _context;

    public GetPatientAppointmentsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AppointmentDto>> Handle(GetPatientAppointmentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Where(a => a.PatientId == request.PatientId)
            .OrderByDescending(a => a.AppointmentDate)
            .Select(a => new AppointmentDto(
                a.Id,
                a.PatientId,
                $"{a.Patient!.FirstName} {a.Patient.LastName}",
                a.AppointmentDate,
                a.Type.ToString(),
                a.Status.ToString(),
                a.Notes
            ))
            .ToListAsync(cancellationToken);
    }
}
