using CarenexaApp.Domain.Entities;
using CarenexaApp.Domain.Enums;
using CarenexaApp.Application.Common.Interfaces;
using MediatR;

namespace CarenexaApp.Application.Appointments.Commands;

public record CreateAppointmentCommand(
    Guid PatientId,
    Guid DoctorId,
    DateTime AppointmentDate,
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
            AppointmentDate = request.AppointmentDate,
            Status = AppointmentStatus.Booked,
            Notes = request.Notes
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);

        return appointment.Id;
    }
}
