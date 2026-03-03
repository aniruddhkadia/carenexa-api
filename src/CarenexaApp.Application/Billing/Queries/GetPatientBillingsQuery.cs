using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Billing.Queries;

public record GetPatientBillingsQuery(Guid PatientId) : IRequest<List<BillingDto>>;

public record BillingDto(
    Guid Id,
    Guid AppointmentId,
    decimal TotalAmount,
    decimal PaidAmount,
    string PaymentStatus,
    string InvoiceNumber,
    DateTime CreatedAt
);

public class GetPatientBillingsQueryHandler : IRequestHandler<GetPatientBillingsQuery, List<BillingDto>>
{
    private readonly IAppDbContext _context;

    public GetPatientBillingsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BillingDto>> Handle(GetPatientBillingsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Billings
            .Where(b => b.PatientId == request.PatientId)
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => new BillingDto(
                b.Id,
                b.AppointmentId,
                b.TotalAmount,
                b.PaidAmount,
                b.PaymentStatus.ToString(),
                b.InvoiceNumber,
                b.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}
