using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.MedicalRecords.Commands;

public record DeleteMedicalRecordCommand(Guid Id) : IRequest<bool>;

public class DeleteMedicalRecordCommandHandler : IRequestHandler<DeleteMedicalRecordCommand, bool>
{
    private readonly IAppDbContext _context;

    public DeleteMedicalRecordCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteMedicalRecordCommand request, CancellationToken cancellationToken)
    {
        var record = await _context.MedicalRecords
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (record == null) return false;

        _context.MedicalRecords.Remove(record);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
