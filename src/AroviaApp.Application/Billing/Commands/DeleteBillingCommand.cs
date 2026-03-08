using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Billing.Commands;

public record DeleteBillingCommand(Guid Id) : IRequest<bool>;

public class DeleteBillingCommandHandler : IRequestHandler<DeleteBillingCommand, bool>
{
    private readonly IAppDbContext _context;

    public DeleteBillingCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteBillingCommand request, CancellationToken cancellationToken)
    {
        var billing = await _context.Billings
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (billing == null) return false;

        _context.Billings.Remove(billing);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
