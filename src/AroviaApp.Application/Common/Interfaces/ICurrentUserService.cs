namespace AroviaApp.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    Guid? ClinicId { get; }
    string? Role { get; }
}
