namespace AroviaApp.Domain.Entities;

public class AuditLog : BaseEntity
{
    public Guid? UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public string IPAddress { get; set; } = string.Empty;

    // Navigation properties
    public User? User { get; set; }
}
