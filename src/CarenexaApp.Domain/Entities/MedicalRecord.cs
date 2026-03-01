namespace CarenexaApp.Domain.Entities;

public class MedicalRecord : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Prescription { get; set; } = string.Empty;
    public string LabNotes { get; set; } = string.Empty;
    public DateTime? FollowUpDate { get; set; }

    // Navigation properties
    public Patient? Patient { get; set; }
    public User? Doctor { get; set; }
}
