using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.MedicalRecords.Queries;

public record GetMedicinesQuery : IRequest<List<MedicineDto>>;

public class GetMedicinesQueryHandler : IRequestHandler<GetMedicinesQuery, List<MedicineDto>>
{
    private readonly IAppDbContext _context;

    public GetMedicinesQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MedicineDto>> Handle(GetMedicinesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Medicines
            .OrderBy(m => m.BrandName)
            .Select(m => new MedicineDto(
                m.Id, 
                m.GenericName, 
                m.BrandName, 
                m.Strength, 
                m.Form,
                m.IsActive,
                m.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
