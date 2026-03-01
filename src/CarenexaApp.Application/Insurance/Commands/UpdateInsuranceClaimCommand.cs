using CarenexaApp.Domain.Enums;
using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Insurance.Commands;

public record UpdateInsuranceClaimCommand(
    Guid Id,
    ClaimStatus Status,
    DateTime? ApprovedDate
) : IRequest<bool>;

public class UpdateInsuranceClaimCommandHandler : IRequestHandler<UpdateInsuranceClaimCommand, bool>
{
    private readonly IAppDbContext _context;

    public UpdateInsuranceClaimCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateInsuranceClaimCommand request, CancellationToken cancellationToken)
    {
        var claim = await _context.InsuranceClaims
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (claim == null) return false;

        claim.Status = request.Status;
        claim.ApprovedDate = request.ApprovedDate;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
