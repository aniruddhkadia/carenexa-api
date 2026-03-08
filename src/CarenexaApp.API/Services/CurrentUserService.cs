using System.Security.Claims;
using AroviaApp.Application.Common.Interfaces;

namespace AroviaApp.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub")
                          ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("id")
                          ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("uid")
                          ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("userid")
                          ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("user_id")
                          ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) // Added fallback
                          ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Upn) // Added fallback
                          ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            return Guid.TryParse(userIdStr, out var userId) ? userId : null;
        }
    }

    public Guid? ClinicId
    {
        get
        {
            var clinicIdStr = _httpContextAccessor.HttpContext?.User?.FindFirstValue("ClinicId")
                            ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("clinic_id");
            return Guid.TryParse(clinicIdStr, out var clinicId) ? clinicId : null;
        }
    }

    public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role)
                        ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("role");
}
