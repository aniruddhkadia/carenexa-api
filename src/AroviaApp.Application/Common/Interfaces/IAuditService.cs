using AroviaApp.Domain.Entities;

namespace AroviaApp.Application.Common.Interfaces;

public interface IAuditService
{
    Task LogAsync(AuditLog auditLog);
    Task LogActionAsync(Guid? userId, string action, string entity, string details);
}
