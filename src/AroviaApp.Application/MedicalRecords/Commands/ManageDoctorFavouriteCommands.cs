using AroviaApp.Application.Common.Interfaces;
using AroviaApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.MedicalRecords.Commands;

public record AddDoctorFavouriteCommand(Guid DoctorId, Guid MedicineId) : IRequest<bool>;

public class AddDoctorFavouriteCommandHandler : IRequestHandler<AddDoctorFavouriteCommand, bool>
{
    private readonly IAppDbContext _context;

    public AddDoctorFavouriteCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AddDoctorFavouriteCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.DoctorFavourites.AnyAsync(f => f.DoctorId == request.DoctorId && f.MedicineId == request.MedicineId, cancellationToken);
        if (exists) return true;

        _context.DoctorFavourites.Add(new DoctorFavourite
        {
            DoctorId = request.DoctorId,
            MedicineId = request.MedicineId
        });
        
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public record RemoveDoctorFavouriteCommand(Guid DoctorId, Guid MedicineId) : IRequest<bool>;

public class RemoveDoctorFavouriteCommandHandler : IRequestHandler<RemoveDoctorFavouriteCommand, bool>
{
    private readonly IAppDbContext _context;

    public RemoveDoctorFavouriteCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(RemoveDoctorFavouriteCommand request, CancellationToken cancellationToken)
    {
        var favourite = await _context.DoctorFavourites.FirstOrDefaultAsync(f => f.DoctorId == request.DoctorId && f.MedicineId == request.MedicineId, cancellationToken);
        if (favourite == null) return false;

        _context.DoctorFavourites.Remove(favourite);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
