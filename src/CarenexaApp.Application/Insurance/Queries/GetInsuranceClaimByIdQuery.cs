using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Insurance.Queries;

public record GetInsuranceClaimByIdQuery(Guid Id) : IRequest<InsuranceClaimDto?>;

public class GetInsuranceClaimByIdQueryHandler : IRequestHandler<GetInsuranceClaimByIdQuery, InsuranceClaimDto?>
{
    private readonly IAppDbContext _context;

    public GetInsuranceClaimByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<InsuranceClaimDto?> Handle(GetInsuranceClaimByIdQuery request, CancellationToken cancellationToken)
    {
        var claim = await _context.InsuranceClaims
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (claim == null) return null;

        return new InsuranceClaimDto(
            claim.Id,
            claim.InsuranceCompany,
            claim.ClaimAmount,
            claim.Status.ToString(),
            claim.SubmittedDate,
            claim.ApprovedDate
        );
    }
}
