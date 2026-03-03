using CarenexaApp.Application.Common.Interfaces;
using CarenexaApp.Domain.Entities;
using CarenexaApp.Infrastructure.Data;

namespace CarenexaApp.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly AppDbContext _context;

    public AuditService(AppDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(AuditLog auditLog)
    {
        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }

    public async Task LogActionAsync(Guid? userId, string action, string entity, string details)
    {
        var auditLog = new AuditLog
        {
            UserId = userId,
            Action = action,
            Entity = entity,
            CreatedAt = DateTime.UtcNow,
            IPAddress = "Internal" // Could be improved with IHttpContextAccessor
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }
}
