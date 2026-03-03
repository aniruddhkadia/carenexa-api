using CarenexaApp.Domain.Entities;
using CarenexaApp.Domain.Enums;
using CarenexaApp.Application.Common.Interfaces;
using MediatR;

namespace CarenexaApp.Application.Billing.Commands;

public record CreateInvoiceCommand(
    Guid PatientId,
    Guid AppointmentId,
    decimal TotalAmount,
    string PaymentMethod
) : IRequest<Guid>;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateInvoiceCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var billing = new Domain.Entities.Billing
        {
            PatientId = request.PatientId,
            AppointmentId = request.AppointmentId,
            TotalAmount = request.TotalAmount,
            PaidAmount = 0,
            PaymentStatus = PaymentStatus.Pending,
            PaymentMethod = request.PaymentMethod,
            InvoiceNumber = $"INV-{DateTime.UtcNow.Ticks}"
        };

        _context.Billings.Add(billing);
        await _context.SaveChangesAsync(cancellationToken);

        return billing.Id;
    }
}
