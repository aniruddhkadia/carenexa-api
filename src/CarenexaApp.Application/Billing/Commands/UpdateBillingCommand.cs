using CarenexaApp.Domain.Enums;
using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Billing.Commands;

public record UpdateBillingCommand(
    Guid Id,
    decimal TotalAmount,
    decimal PaidAmount,
    PaymentStatus PaymentStatus,
    string PaymentMethod
) : IRequest<bool>;

public class UpdateBillingCommandHandler : IRequestHandler<UpdateBillingCommand, bool>
{
    private readonly IAppDbContext _context;

    public UpdateBillingCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateBillingCommand request, CancellationToken cancellationToken)
    {
        var billing = await _context.Billings
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (billing == null) return false;

        billing.TotalAmount = request.TotalAmount;
        billing.PaidAmount = request.PaidAmount;
        billing.PaymentStatus = request.PaymentStatus;
        billing.PaymentMethod = request.PaymentMethod;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
