using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Appointments.Queries;

public record GetAppointmentByIdQuery(Guid Id, Guid DoctorId) : IRequest<AppointmentDto?>;

public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto?>
{
    private readonly IAppDbContext _context;

    public GetAppointmentByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<AppointmentDto?> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.Id == request.Id && a.DoctorId == request.DoctorId, cancellationToken);

        if (appointment == null) return null;

        return new AppointmentDto(
            appointment.Id,
            appointment.PatientId,
            $"{appointment.Patient!.FirstName} {appointment.Patient.LastName}",
            appointment.AppointmentDate,
            appointment.Type.ToString(),
            appointment.Status.ToString(),
            appointment.Notes
        );
    }
}
