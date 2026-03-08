using AroviaApp.Application.Common.Interfaces;
using AroviaApp.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Patients.Queries;

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
    string BloodGroup,
    string CreatedByUserName
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
            .Include(p => p.CreatedByUser)
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
                p.BloodGroup,
                p.CreatedByUser != null && !string.IsNullOrEmpty(p.CreatedByUser.FullName) 
                    ? p.CreatedByUser.FullName 
                    : (p.CreatedByUserId == Guid.Empty ? "System" : "Unknown User")
            ))
            .ToListAsync(cancellationToken);

        return new PaginatedResult<PatientDto>(items, totalCount, request.Page, request.PageSize);
    }
}
