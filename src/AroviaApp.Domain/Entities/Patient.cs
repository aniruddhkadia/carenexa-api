namespace AroviaApp.Domain.Entities;

public class Patient : BaseEntity
{
    public Guid ClinicId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DOB { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty;
    public Guid? InsuranceId { get; set; }
    public Guid CreatedByUserId { get; set; }

    // Navigation properties
    public Clinic? Clinic { get; set; }
    public User? CreatedByUser { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
}
