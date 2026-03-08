using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Billing.Queries;

public record GetBillingByIdQuery(Guid Id) : IRequest<BillingDto?>;

public class GetBillingByIdQueryHandler : IRequestHandler<GetBillingByIdQuery, BillingDto?>
{
    private readonly IAppDbContext _context;

    public GetBillingByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<BillingDto?> Handle(GetBillingByIdQuery request, CancellationToken cancellationToken)
    {
        var billing = await _context.Billings
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (billing == null) return null;

        return new BillingDto(
            billing.Id,
            billing.AppointmentId,
            billing.TotalAmount,
            billing.PaidAmount,
            billing.PaymentStatus.ToString(),
            billing.InvoiceNumber,
            billing.CreatedAt
        );
    }
}
