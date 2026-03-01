using CarenexaApp.Domain.Entities;
using CarenexaApp.Application.Common.Interfaces;
using MediatR;

namespace CarenexaApp.Application.MedicalRecords.Commands;

public record CreateMedicalRecordCommand(
    Guid PatientId,
    Guid DoctorId,
    string Diagnosis,
    string Prescription,
    string LabNotes,
    DateTime? FollowUpDate
) : IRequest<Guid>;

public class CreateMedicalRecordCommandHandler : IRequestHandler<CreateMedicalRecordCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateMedicalRecordCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateMedicalRecordCommand request, CancellationToken cancellationToken)
    {
        var record = new MedicalRecord
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            Diagnosis = request.Diagnosis,
            Prescription = request.Prescription,
            LabNotes = request.LabNotes,
            FollowUpDate = request.FollowUpDate
        };

        _context.MedicalRecords.Add(record);
        await _context.SaveChangesAsync(cancellationToken);

        return record.Id;
    }
}
