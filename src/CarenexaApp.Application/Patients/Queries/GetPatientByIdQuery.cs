using CarenexaApp.Domain.Entities;
using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Patients.Queries;

public record GetPatientByIdQuery(Guid Id, Guid ClinicId) : IRequest<PatientDetailDto?>;

public record PatientDetailDto(
    Guid Id,
    string FirstName,
    string LastName,
    DateTime DOB,
    string Gender,
    string Phone,
    string Email,
    string Address,
    string BloodGroup,
    Guid? InsuranceId
);

public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, PatientDetailDto?>
{
    private readonly IAppDbContext _context;

    public GetPatientByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PatientDetailDto?> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.ClinicId == request.ClinicId, cancellationToken);

        if (patient == null) return null;

        return new PatientDetailDto(
            patient.Id,
            patient.FirstName,
            patient.LastName,
            patient.DOB,
            patient.Gender,
            patient.Phone,
            patient.Email,
            patient.Address,
            patient.BloodGroup,
            patient.InsuranceId
        );
    }
}
