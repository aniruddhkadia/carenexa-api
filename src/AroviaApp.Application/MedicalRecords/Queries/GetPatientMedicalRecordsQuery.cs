using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.MedicalRecords.Queries;

public record GetPatientMedicalRecordsQuery(Guid PatientId) : IRequest<List<MedicalRecordDto>>;

public record MedicalRecordDto(
    Guid Id,
    string Diagnosis,
    string Prescription,
    string Advice,
    string LabNotes,
    DateTime? FollowUpDate,
    DateTime CreatedAt,
    string DoctorName,
    Guid? AppointmentId,
    string ChiefComplaint,
    string History,
    string Status,
    string Weight,
    string BP,
    string Temp,
    string Pulse
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
            .Where(m => m.PatientId == request.PatientId && m.Status == AroviaApp.Domain.Enums.MedicalRecordStatus.Completed)
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new MedicalRecordDto(
                m.Id,
                m.Diagnosis,
                m.Prescription,
                m.Advice,
                m.LabNotes,
                m.FollowUpDate,
                m.CreatedAt,
                m.Doctor!.FullName,
                m.AppointmentId,
                m.ChiefComplaint,
                m.History,
                m.Status.ToString(),
                m.Weight,
                m.BP,
                m.Temp,
                m.Pulse
            ))
            .ToListAsync(cancellationToken);
    }
}
