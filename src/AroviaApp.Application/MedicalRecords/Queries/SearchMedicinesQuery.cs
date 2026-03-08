using AroviaApp.Application.Common.Models;
using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.MedicalRecords.Queries;

public record SearchMedicinesQuery(string Query, int Page = 1, int PageSize = 50) : IRequest<PaginatedResult<MedicineDto>>;

public class SearchMedicinesQueryHandler : IRequestHandler<SearchMedicinesQuery, PaginatedResult<MedicineDto>>
{
    private readonly IAppDbContext _context;

    public SearchMedicinesQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<MedicineDto>> Handle(SearchMedicinesQuery request, CancellationToken cancellationToken)
    {
        var searchTerm = request.Query?.ToLower() ?? "";
        
        var query = _context.Medicines
            .AsNoTracking()
            .Where(m => string.IsNullOrWhiteSpace(searchTerm) || 
                         m.GenericName.ToLower().Contains(searchTerm) || 
                         m.BrandName.ToLower().Contains(searchTerm));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(m => m.BrandName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(m => new MedicineDto(m.Id, m.GenericName, m.BrandName, m.Strength, m.Form, m.IsActive, m.CreatedAt))
            .ToListAsync(cancellationToken);

        return new PaginatedResult<MedicineDto>(items, totalCount, request.Page, request.PageSize);
    }
}
