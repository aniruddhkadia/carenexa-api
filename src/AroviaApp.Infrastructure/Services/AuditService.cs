using AroviaApp.Application.Common.Interfaces;
using AroviaApp.Domain.Entities;
using AroviaApp.Infrastructure.Data;

namespace AroviaApp.Infrastructure.Services;

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
