using CarenexaApp.Domain.Entities;
using CarenexaApp.Application.Common.Interfaces;
using MediatR;

namespace CarenexaApp.Application.Patients.Commands;

public record CreatePatientCommand(
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
) : IRequest<Guid>;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreatePatientCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = new Patient
        {
            ClinicId = request.ClinicId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DOB = request.DOB,
            Gender = request.Gender,
            Phone = request.Phone,
            Email = request.Email,
            Address = request.Address,
            BloodGroup = request.BloodGroup,
            InsuranceId = request.InsuranceId
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync(cancellationToken);

        return patient.Id;
    }
}
