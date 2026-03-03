using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.MedicalRecords.Queries;

public record GetMedicalRecordByIdQuery(Guid Id) : IRequest<MedicalRecordDto?>;

public class GetMedicalRecordByIdQueryHandler : IRequestHandler<GetMedicalRecordByIdQuery, MedicalRecordDto?>
{
    private readonly IAppDbContext _context;

    public GetMedicalRecordByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<MedicalRecordDto?> Handle(GetMedicalRecordByIdQuery request, CancellationToken cancellationToken)
    {
        var record = await _context.MedicalRecords
            .Include(m => m.Doctor)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (record == null) return null;

        return new MedicalRecordDto(
            record.Id,
            record.Diagnosis,
            record.Prescription,
            record.Advice,
            record.LabNotes,
            record.FollowUpDate,
            record.CreatedAt,
            record.Doctor!.FullName,
            record.AppointmentId,
            record.ChiefComplaint,
            record.History,
            record.Status.ToString(),
            record.Weight,
            record.BP,
            record.Temp,
            record.Pulse
        );
    }
}
