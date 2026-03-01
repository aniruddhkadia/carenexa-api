using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Appointments.Commands;

public record DeleteAppointmentCommand(Guid Id, Guid DoctorId) : IRequest<bool>;

public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, bool>
{
    private readonly IAppDbContext _context;

    public DeleteAppointmentCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == request.Id && a.DoctorId == request.DoctorId, cancellationToken);

        if (appointment == null) return false;

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
