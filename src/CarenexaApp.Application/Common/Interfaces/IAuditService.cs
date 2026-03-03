using CarenexaApp.Domain.Entities;

namespace CarenexaApp.Application.Common.Interfaces;

public interface IAuditService
{
    Task LogAsync(AuditLog auditLog);
    Task LogActionAsync(Guid? userId, string action, string entity, string details);
}
