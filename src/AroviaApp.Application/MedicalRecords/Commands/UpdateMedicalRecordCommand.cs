using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.MedicalRecords.Commands;

public record UpdateMedicalRecordCommand(
    Guid Id,
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
) : IRequest<bool>;

public class UpdateMedicalRecordCommandHandler : IRequestHandler<UpdateMedicalRecordCommand, bool>
{
    private readonly IAppDbContext _context;

    public UpdateMedicalRecordCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateMedicalRecordCommand request, CancellationToken cancellationToken)
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

        var record = await _context.MedicalRecords
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (record == null) return false;

        record.ChiefComplaint = request.ChiefComplaint;
        record.History = request.History;
        record.Diagnosis = request.Diagnosis;
        record.Prescription = request.Prescription;
        record.Advice = request.Advice;
        record.LabNotes = request.LabNotes;
        record.FollowUpDate = request.FollowUpDate;
        record.Weight = request.Weight;
        record.BP = request.BP;
        record.Temp = request.Temp;
        record.Pulse = request.Pulse;
        record.Status = request.Status;

        // Auto-complete the associated appointment if the record is COMPLETED
        if (record.Status == AroviaApp.Domain.Enums.MedicalRecordStatus.Completed && record.AppointmentId != null)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == record.AppointmentId, cancellationToken);
            
            if (appointment != null)
            {
                appointment.Status = AroviaApp.Domain.Enums.AppointmentStatus.Completed;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
