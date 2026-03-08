using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.MedicalRecords.Commands;

public record UpdateMedicineCommand(
    Guid Id,
    string GenericName, 
    string BrandName, 
    string Strength, 
    string Form) : IRequest<bool>;

public class UpdateMedicineCommandHandler : IRequestHandler<UpdateMedicineCommand, bool>
{
    private readonly IAppDbContext _context;

    public UpdateMedicineCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateMedicineCommand request, CancellationToken cancellationToken)
    {
        var medicine = await _context.Medicines
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (medicine == null) return false;

        // Check for duplicates (same name and strength but different ID)
        var exists = await _context.Medicines
            .AnyAsync(m => m.Id != request.Id && 
                           m.BrandName.ToLower() == request.BrandName.ToLower() && 
                           m.Strength.ToLower() == request.Strength.ToLower(), 
                       cancellationToken);

        if (exists)
        {
            throw new Exception("Another medicine with this name and strength already exists.");
        }

        medicine.GenericName = request.GenericName;
        medicine.BrandName = request.BrandName;
        medicine.Strength = request.Strength;
        medicine.Form = request.Form;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
