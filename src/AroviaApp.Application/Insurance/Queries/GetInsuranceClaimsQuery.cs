using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Insurance.Queries;

public record GetInsuranceClaimsQuery(Guid PatientId) : IRequest<List<InsuranceClaimDto>>;

public record InsuranceClaimDto(
    Guid Id,
    string InsuranceCompany,
    decimal ClaimAmount,
    string Status,
    DateTime SubmittedDate,
    DateTime? ApprovedDate
);

public class GetInsuranceClaimsQueryHandler : IRequestHandler<GetInsuranceClaimsQuery, List<InsuranceClaimDto>>
{
    private readonly IAppDbContext _context;

    public GetInsuranceClaimsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<InsuranceClaimDto>> Handle(GetInsuranceClaimsQuery request, CancellationToken cancellationToken)
    {
        return await _context.InsuranceClaims
            .Where(c => c.PatientId == request.PatientId)
            .OrderByDescending(c => c.SubmittedDate)
            .Select(c => new InsuranceClaimDto(
                c.Id,
                c.InsuranceCompany,
                c.ClaimAmount,
                c.Status.ToString(),
                c.SubmittedDate,
                c.ApprovedDate
            ))
            .ToListAsync(cancellationToken);
    }
}
