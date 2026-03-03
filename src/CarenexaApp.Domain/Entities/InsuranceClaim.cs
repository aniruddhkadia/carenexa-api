using CarenexaApp.Domain.Enums;

namespace CarenexaApp.Domain.Entities;

public class InsuranceClaim : BaseEntity
{
    public Guid PatientId { get; set; }
    public string InsuranceCompany { get; set; } = string.Empty;
    public decimal ClaimAmount { get; set; }
    public ClaimStatus Status { get; set; }
    public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedDate { get; set; }

    // Navigation properties
    public Patient? Patient { get; set; }
}
