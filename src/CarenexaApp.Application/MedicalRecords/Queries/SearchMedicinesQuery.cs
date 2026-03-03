using CarenexaApp.Application.Common.Interfaces;
using CarenexaApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.MedicalRecords.Queries;

public record SearchMedicinesQuery(string Query) : IRequest<List<MedicineDto>>;

<<<<<<< HEAD
=======
public record MedicineDto(Guid Id, string GenericName, string BrandName, string Strength, string Form);

>>>>>>> 6829967ddade774c1ea73506d65fb9d746b4b00c
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
<<<<<<< HEAD
            .Select(m => new MedicineDto(m.Id, m.GenericName, m.BrandName, m.Strength, m.Form, m.IsActive, m.CreatedAt))
=======
            .Select(m => new MedicineDto(m.Id, m.GenericName, m.BrandName, m.Strength, m.Form))
>>>>>>> 6829967ddade774c1ea73506d65fb9d746b4b00c
            .Take(20)
            .ToListAsync(cancellationToken);
    }
}
