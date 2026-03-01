using CarenexaApp.Domain.Entities;
using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Patients.Queries;

public record GetPatientsQuery(
    Guid ClinicId, 
    int Page = 1, 
    int PageSize = 20, 
    string? SearchTerm = null
) : IRequest<PaginatedResult<PatientDto>>;

public record PatientDto(
    Guid Id,
    string FirstName,
    string LastName,
    DateTime DOB,
    string Gender,
    string Phone,
    string Email,
    string BloodGroup
);

public record PaginatedResult<T>(
    List<T> Items,
    int TotalCount,
    int Page,
    int PageSize
);

public class GetPatientsQueryHandler : IRequestHandler<GetPatientsQuery, PaginatedResult<PatientDto>>
{
    private readonly IAppDbContext _context;

    public GetPatientsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<PatientDto>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Patients
            .Where(p => p.ClinicId == request.ClinicId);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();
            query = query.Where(p => 
                p.FirstName.ToLower().Contains(search) || 
                p.LastName.ToLower().Contains(search) || 
                p.Phone.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new PatientDto(
                p.Id,
                p.FirstName,
                p.LastName,
                p.DOB,
                p.Gender,
                p.Phone,
                p.Email,
                p.BloodGroup
            ))
            .ToListAsync(cancellationToken);

        return new PaginatedResult<PatientDto>(items, totalCount, request.Page, request.PageSize);
    }
}
