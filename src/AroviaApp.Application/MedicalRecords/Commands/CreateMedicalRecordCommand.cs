using AroviaApp.Domain.Entities;
using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.MedicalRecords.Commands;

public record CreateMedicalRecordCommand(
    Guid PatientId,
    Guid DoctorId,
    Guid ClinicId,
    Guid? AppointmentId,
    string ChiefComplaint,
    string History,
    string Diagnosis,
    string Prescription,
    string Advice,
    string LabNotes,
    DateTime? FollowUpDate,
    string Weight,
    string BP,
    string Temp,
    string Pulse,
    AroviaApp.Domain.Enums.MedicalRecordStatus Status
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
        if (request.Status == AroviaApp.Domain.Enums.MedicalRecordStatus.Completed)
        {
            if (string.IsNullOrWhiteSpace(request.ChiefComplaint))
            {
                throw new InvalidOperationException("Chief Complaint & History is required to complete the visit.");
            }
            if (!request.FollowUpDate.HasValue)
            {
                throw new InvalidOperationException("Schedule Follow-up is required to complete the visit.");
            }
        }

        // Auto-complete the associated appointment if the record is COMPLETED
        if (request.Status == AroviaApp.Domain.Enums.MedicalRecordStatus.Completed && request.AppointmentId != null && request.AppointmentId != Guid.Empty)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);
            
            if (appointment != null)
            {
                appointment.Status = AroviaApp.Domain.Enums.AppointmentStatus.Completed;
            }
        }

        var record = new MedicalRecord
        {
            PatientId = request.PatientId,
            DoctorId = request.DoctorId,
            ClinicId = request.ClinicId,
            AppointmentId = request.AppointmentId == Guid.Empty ? null : request.AppointmentId,
            ChiefComplaint = request.ChiefComplaint,
            History = request.History,
            Diagnosis = request.Diagnosis,
            Prescription = request.Prescription,
            Advice = request.Advice,
            LabNotes = request.LabNotes,
            FollowUpDate = request.FollowUpDate,
            Weight = request.Weight,
            BP = request.BP,
            Temp = request.Temp,
            Pulse = request.Pulse,
            Status = request.Status
        };

        _context.MedicalRecords.Add(record);
        await _context.SaveChangesAsync(cancellationToken);

        return record.Id;
    }
}
