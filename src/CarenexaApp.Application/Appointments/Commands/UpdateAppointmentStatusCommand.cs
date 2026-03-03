using CarenexaApp.Domain.Enums;
using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Appointments.Commands;

public record UpdateAppointmentStatusCommand(
    Guid Id,
    Guid DoctorId,
    AppointmentStatus Status
) : IRequest<bool>;

public class UpdateAppointmentStatusCommandHandler : IRequestHandler<UpdateAppointmentStatusCommand, bool>
{
    private readonly IAppDbContext _context;

    public UpdateAppointmentStatusCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateAppointmentStatusCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == request.Id && a.DoctorId == request.DoctorId, cancellationToken);

        if (appointment == null) return false;

        appointment.Status = request.Status;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
