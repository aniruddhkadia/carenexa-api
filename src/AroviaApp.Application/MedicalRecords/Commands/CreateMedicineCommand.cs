using AroviaApp.Application.Common.Interfaces;
using AroviaApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.MedicalRecords.Commands;

public record CreateMedicineCommand(
    string GenericName, 
    string BrandName, 
    string Strength, 
    string Form) : IRequest<Guid>;

public class CreateMedicineCommandHandler : IRequestHandler<CreateMedicineCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateMedicineCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateMedicineCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.Medicines
            .AnyAsync(m => m.BrandName.ToLower() == request.BrandName.ToLower() && 
                           m.Strength.ToLower() == request.Strength.ToLower(), 
                      cancellationToken);

        if (exists)
        {
            throw new Exception("Medicine with this name and strength already exists.");
        }

        var medicine = new Medicine
        {
            Id = Guid.NewGuid(),
            GenericName = request.GenericName,
            BrandName = request.BrandName,
            Strength = request.Strength,
            Form = request.Form,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Medicines.Add(medicine);
        await _context.SaveChangesAsync(cancellationToken);

        return medicine.Id;
    }
}
