using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.MedicalRecords.Queries;

public record GetAllMedicalRecordsQuery : IRequest<List<MedicalRecordListDto>>;

public record MedicalRecordListDto(
    Guid Id,
    string PatientFullName,
    string Diagnosis,
    string DoctorName,
    DateTime CreatedAt,
    string Status
);

public class GetAllMedicalRecordsQueryHandler : IRequestHandler<GetAllMedicalRecordsQuery, List<MedicalRecordListDto>>
{
    private readonly IAppDbContext _context;

    public GetAllMedicalRecordsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MedicalRecordListDto>> Handle(GetAllMedicalRecordsQuery request, CancellationToken cancellationToken)
    {
        return await _context.MedicalRecords
            .Include(m => m.Patient)
            .Include(m => m.Doctor)
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new MedicalRecordListDto(
                m.Id,
                m.Patient!.FirstName + " " + m.Patient.LastName,
                m.Diagnosis,
                m.Doctor!.FullName,
                m.CreatedAt,
                m.Status.ToString()
            ))
            .Take(100)
            .ToListAsync(cancellationToken);
    }
}
