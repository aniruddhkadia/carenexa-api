using CarenexaApp.Domain.Enums;

namespace CarenexaApp.Domain.Entities;

public class Billing : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid AppointmentId { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string InvoiceNumber { get; set; } = string.Empty;

    // Navigation properties
    public Patient? Patient { get; set; }
    public Appointment? Appointment { get; set; }
}
