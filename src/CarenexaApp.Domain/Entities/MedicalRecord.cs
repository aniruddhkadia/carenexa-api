using CarenexaApp.Domain.Enums;

namespace CarenexaApp.Domain.Entities;

public class MedicalRecord : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public Guid ClinicId { get; set; }
    public Guid? AppointmentId { get; set; }
    
    public string ChiefComplaint { get; set; } = string.Empty;
    public string History { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string Prescription { get; set; } = "[]"; // JSON array default
    public string Advice { get; set; } = string.Empty;
    public string LabNotes { get; set; } = string.Empty;
    public DateTime? FollowUpDate { get; set; }
    public string Weight { get; set; } = string.Empty;
    public string BP { get; set; } = string.Empty;
    public string Temp { get; set; } = string.Empty;
    public string Pulse { get; set; } = string.Empty;
    public MedicalRecordStatus Status { get; set; } = MedicalRecordStatus.Draft;

    // Navigation properties
    public Patient? Patient { get; set; }
    public User? Doctor { get; set; }
    public Clinic? Clinic { get; set; }
    public Appointment? Appointment { get; set; }
}
