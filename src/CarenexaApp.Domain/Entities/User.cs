using CarenexaApp.Domain.Enums;

namespace CarenexaApp.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public Guid ClinicId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Clinic? Clinic { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
