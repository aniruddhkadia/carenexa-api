using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Insurance.Commands;

public record DeleteInsuranceClaimCommand(Guid Id) : IRequest<bool>;

public class DeleteInsuranceClaimCommandHandler : IRequestHandler<DeleteInsuranceClaimCommand, bool>
{
    private readonly IAppDbContext _context;

    public DeleteInsuranceClaimCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteInsuranceClaimCommand request, CancellationToken cancellationToken)
    {
        var claim = await _context.InsuranceClaims
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (claim == null) return false;

        _context.InsuranceClaims.Remove(claim);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
