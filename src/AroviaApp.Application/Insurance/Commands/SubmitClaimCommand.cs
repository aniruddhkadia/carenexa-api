using AroviaApp.Domain.Entities;
using AroviaApp.Domain.Enums;
using AroviaApp.Application.Common.Interfaces;
using MediatR;

namespace AroviaApp.Application.Insurance.Commands;

public record SubmitClaimCommand(
    Guid PatientId,
    string InsuranceCompany,
    decimal ClaimAmount
) : IRequest<Guid>;

public class SubmitClaimCommandHandler : IRequestHandler<SubmitClaimCommand, Guid>
{
    private readonly IAppDbContext _context;

    public SubmitClaimCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(SubmitClaimCommand request, CancellationToken cancellationToken)
    {
        var claim = new InsuranceClaim
        {
            PatientId = request.PatientId,
            InsuranceCompany = request.InsuranceCompany,
            ClaimAmount = request.ClaimAmount,
            Status = ClaimStatus.Pending,
            SubmittedDate = DateTime.UtcNow
        };

        _context.InsuranceClaims.Add(claim);
        await _context.SaveChangesAsync(cancellationToken);

        return claim.Id;
    }
}
