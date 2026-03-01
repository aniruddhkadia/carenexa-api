using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.MedicalRecords.Commands;

public record UpdateMedicalRecordCommand(
    Guid Id,
    string Diagnosis,
    string Prescription,
    string LabNotes,
    DateTime? FollowUpDate
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
        var record = await _context.MedicalRecords
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (record == null) return false;

        record.Diagnosis = request.Diagnosis;
        record.Prescription = request.Prescription;
        record.LabNotes = request.LabNotes;
        record.FollowUpDate = request.FollowUpDate;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
