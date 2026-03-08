using AroviaApp.Application.Common.Models;
using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.MedicalRecords.Queries;

public record GetMedicinesQuery(int Page = 1, int PageSize = 50) : IRequest<PaginatedResult<MedicineDto>>;

public class GetMedicinesQueryHandler : IRequestHandler<GetMedicinesQuery, PaginatedResult<MedicineDto>>
{
    private readonly IAppDbContext _context;

    public GetMedicinesQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<MedicineDto>> Handle(GetMedicinesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Medicines.AsNoTracking();
        
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(m => m.BrandName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(m => new MedicineDto(
                m.Id, 
                m.GenericName, 
                m.BrandName, 
                m.Strength, 
                m.Form,
                m.IsActive,
                m.CreatedAt))
            .ToListAsync(cancellationToken);

        return new PaginatedResult<MedicineDto>(items, totalCount, request.Page, request.PageSize);
    }
}
