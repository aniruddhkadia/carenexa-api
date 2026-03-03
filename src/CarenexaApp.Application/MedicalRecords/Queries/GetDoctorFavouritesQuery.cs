using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.MedicalRecords.Queries;

public record GetDoctorFavouritesQuery(Guid DoctorId) : IRequest<List<MedicineDto>>;

public class GetDoctorFavouritesQueryHandler : IRequestHandler<GetDoctorFavouritesQuery, List<MedicineDto>>
{
    private readonly IAppDbContext _context;

    public GetDoctorFavouritesQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MedicineDto>> Handle(GetDoctorFavouritesQuery request, CancellationToken cancellationToken)
    {
        return await _context.DoctorFavourites
            .Include(f => f.Medicine)
            .Where(f => f.DoctorId == request.DoctorId && f.Medicine!.IsActive)
<<<<<<< HEAD
            .Select(f => new MedicineDto(f.Medicine!.Id, f.Medicine.GenericName, f.Medicine.BrandName, f.Medicine.Strength, f.Medicine.Form, f.Medicine.IsActive, f.Medicine.CreatedAt))
=======
            .Select(f => new MedicineDto(f.Medicine!.Id, f.Medicine.GenericName, f.Medicine.BrandName, f.Medicine.Strength, f.Medicine.Form))
>>>>>>> 6829967ddade774c1ea73506d65fb9d746b4b00c
            .ToListAsync(cancellationToken);
    }
}
