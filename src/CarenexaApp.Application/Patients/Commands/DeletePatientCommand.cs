using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Patients.Commands;

public record DeletePatientCommand(Guid Id, Guid ClinicId) : IRequest<bool>;

public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, bool>
{
    private readonly IAppDbContext _context;

    public DeletePatientCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.ClinicId == request.ClinicId, cancellationToken);

        if (patient == null) return false;

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
