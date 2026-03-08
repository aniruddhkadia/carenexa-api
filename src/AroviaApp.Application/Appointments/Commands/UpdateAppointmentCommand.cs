using AroviaApp.Domain.Enums;
using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Appointments.Commands;

public record UpdateAppointmentCommand(
    Guid Id,
    Guid DoctorId,
    DateTime AppointmentDate,
    AppointmentType Type,
    AppointmentStatus Status,
    string Notes
) : IRequest<bool>;

public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, bool>
{
    private readonly IAppDbContext _context;

    public UpdateAppointmentCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == request.Id && a.DoctorId == request.DoctorId, cancellationToken);

        if (appointment == null) return false;

        appointment.AppointmentDate = request.AppointmentDate;
        appointment.Type = request.Type;
        appointment.Status = request.Status;
        appointment.Notes = request.Notes;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
