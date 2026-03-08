using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.MedicalRecords.Commands;

public record DeleteMedicineCommand(Guid Id) : IRequest<bool>;

public class DeleteMedicineCommandHandler : IRequestHandler<DeleteMedicineCommand, bool>
{
    private readonly IAppDbContext _context;

    public DeleteMedicineCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteMedicineCommand request, CancellationToken cancellationToken)
    {
        var medicine = await _context.Medicines
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (medicine == null) return false;

        // Soft delete or hard delete? Usually better to soft delete in healthcare.
        // Looking at Medicine.cs property: IsActive
        
        _context.Medicines.Remove(medicine);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
