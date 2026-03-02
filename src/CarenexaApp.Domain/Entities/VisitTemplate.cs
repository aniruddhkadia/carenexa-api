namespace CarenexaApp.Domain.Entities;

public class VisitTemplate : BaseEntity
{
    public Guid DoctorId { get; set; }
    public Guid ClinicId { get; set; }
    public string TemplateName { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string Advice { get; set; } = string.Empty;
    public string PrescriptionJson { get; set; } = "[]";

    // Navigation properties
    public User? Doctor { get; set; }
    public Clinic? Clinic { get; set; }
}
