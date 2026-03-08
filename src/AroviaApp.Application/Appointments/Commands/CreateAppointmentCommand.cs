using AroviaApp.Domain.Entities;
using AroviaApp.Domain.Enums;
using AroviaApp.Application.Common.Interfaces;
using MediatR;

namespace AroviaApp.Application.Appointments.Commands;

public record CreateAppointmentCommand(
    Guid PatientId,
    Guid DoctorId,
    Guid ClinicId,
    DateTime AppointmentDate,
    AppointmentType Type,
    string Notes
) : IRequest<Guid>;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateAppointmentCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            ClinicId = request.ClinicId,
            AppointmentDate = request.AppointmentDate,
            Type = request.Type,
    Status = AppointmentStatus.Booked,
            Notes = request.Notes
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);

        return appointment.Id;
    }
}
