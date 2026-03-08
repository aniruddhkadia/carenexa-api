namespace AroviaApp.Domain.Entities;

public class Clinic : BaseEntity
{
    public string ClinicName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public Guid? SubscriptionPlanId { get; set; }

    // Navigation properties
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    public ICollection<VisitTemplate> VisitTemplates { get; set; } = new List<VisitTemplate>();
}
