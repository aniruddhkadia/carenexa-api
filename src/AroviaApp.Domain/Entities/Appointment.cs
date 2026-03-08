using AroviaApp.Domain.Enums;

namespace AroviaApp.Domain.Entities;

public class Appointment : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public AppointmentType Type { get; set; }
    public AppointmentStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Guid ClinicId { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation properties
    public Patient? Patient { get; set; }
    public User? Doctor { get; set; }
    public Clinic? Clinic { get; set; }
}
