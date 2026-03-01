using CarenexaApp.Domain.Enums;

namespace CarenexaApp.Domain.Entities;

public class Appointment : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;

    // Navigation properties
    public Patient? Patient { get; set; }
    public User? Doctor { get; set; }
}
