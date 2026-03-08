using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Patients.Queries;

public record SearchPatientsQuery(Guid ClinicId, string Query) : IRequest<List<SearchPatientDto>>;

public record SearchPatientDto(
    Guid Id,
    string FullName,
    string Phone
);

public class SearchPatientsQueryHandler : IRequestHandler<SearchPatientsQuery, List<SearchPatientDto>>
{
    private readonly IAppDbContext _context;

    public SearchPatientsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SearchPatientDto>> Handle(SearchPatientsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            return new List<SearchPatientDto>();

        var search = request.Query.ToLower();

        return await _context.Patients
            .Where(p => p.ClinicId == request.ClinicId &&
                        (p.FirstName.ToLower().Contains(search) ||
                         p.LastName.ToLower().Contains(search) ||
                         p.Phone.Contains(search)))
            .Take(5)
            .Select(p => new SearchPatientDto(
                p.Id,
                $"{p.FirstName} {p.LastName}",
                p.Phone
            ))
            .ToListAsync(cancellationToken);
    }
}
