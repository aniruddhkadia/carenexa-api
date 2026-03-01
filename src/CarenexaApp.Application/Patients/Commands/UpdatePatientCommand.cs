using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Patients.Commands;

public record UpdatePatientCommand(
    Guid Id,
    Guid ClinicId,
    string FirstName,
    string LastName,
    DateTime DOB,
    string Gender,
    string Phone,
    string Email,
    string Address,
    string BloodGroup,
    Guid? InsuranceId
) : IRequest<bool>;

public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, bool>
{
    private readonly IAppDbContext _context;

    public UpdatePatientCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.ClinicId == request.ClinicId, cancellationToken);

        if (patient == null) return false;

        patient.FirstName = request.FirstName;
        patient.LastName = request.LastName;
        patient.DOB = request.DOB;
        patient.Gender = request.Gender;
        patient.Phone = request.Phone;
        patient.Email = request.Email;
        patient.Address = request.Address;
        patient.BloodGroup = request.BloodGroup;
        patient.InsuranceId = request.InsuranceId;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
