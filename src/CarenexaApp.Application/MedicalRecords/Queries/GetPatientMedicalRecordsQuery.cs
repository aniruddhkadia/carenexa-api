using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.MedicalRecords.Queries;

public record GetPatientMedicalRecordsQuery(Guid PatientId) : IRequest<List<MedicalRecordDto>>;

public record MedicalRecordDto(
    Guid Id,
    string Diagnosis,
    string Prescription,
    string LabNotes,
    DateTime? FollowUpDate,
    DateTime CreatedAt,
    string DoctorName
);

public class GetPatientMedicalRecordsQueryHandler : IRequestHandler<GetPatientMedicalRecordsQuery, List<MedicalRecordDto>>
{
    private readonly IAppDbContext _context;

    public GetPatientMedicalRecordsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MedicalRecordDto>> Handle(GetPatientMedicalRecordsQuery request, CancellationToken cancellationToken)
    {
        return await _context.MedicalRecords
            .Include(m => m.Doctor)
            .Where(m => m.PatientId == request.PatientId)
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new MedicalRecordDto(
                m.Id,
                m.Diagnosis,
                m.Prescription,
                m.LabNotes,
                m.FollowUpDate,
                m.CreatedAt,
                m.Doctor!.FullName
            ))
            .ToListAsync(cancellationToken);
    }
}
