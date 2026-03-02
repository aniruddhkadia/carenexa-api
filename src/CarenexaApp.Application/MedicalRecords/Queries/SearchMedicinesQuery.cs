using CarenexaApp.Application.Common.Interfaces;
using CarenexaApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.MedicalRecords.Queries;

public record SearchMedicinesQuery(string Query) : IRequest<List<MedicineDto>>;

public record MedicineDto(Guid Id, string GenericName, string BrandName, string Strength, string Form);

public class SearchMedicinesQueryHandler : IRequestHandler<SearchMedicinesQuery, List<MedicineDto>>
{
    private readonly IAppDbContext _context;

    public SearchMedicinesQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MedicineDto>> Handle(SearchMedicinesQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query) || request.Query.Length < 2)
        {
            return new List<MedicineDto>();
        }

        var searchTerm = request.Query.ToLower();

        return await _context.Medicines
            .Where(m => m.IsActive && 
                        (m.GenericName.ToLower().Contains(searchTerm) || 
                         m.BrandName.ToLower().Contains(searchTerm)))
            .Select(m => new MedicineDto(m.Id, m.GenericName, m.BrandName, m.Strength, m.Form))
            .Take(20)
            .ToListAsync(cancellationToken);
    }
}
