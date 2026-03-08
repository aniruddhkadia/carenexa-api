using System.Security.Claims;
using AroviaApp.Application.Common.Interfaces;
using AroviaApp.Domain.Entities;

namespace AroviaApp.API.Middleware;

public class AuditLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public AuditLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAuditService auditService)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        
        // Skip logging for Auth endpoints as they handle their own logging or are unauthenticated
        if (path.Contains("/api/auth/", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var method = context.Request.Method;
        
        // Log only data-modifying actions
        if (method == HttpMethods.Post || method == HttpMethods.Put || method == HttpMethods.Delete)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var action = method switch
            {
                "POST" => "CREATE",
                "PUT" => "UPDATE",
                "DELETE" => "DELETE",
                _ => "UNKNOWN"
            };

            var entity = context.Request.Path.Value?.Split('/').LastOrDefault() ?? "Unknown";

            await _next(context); // Execute the request first to see if it succeeds

            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
            {
                var auditLog = new AuditLog
                {
                    UserId = Guid.TryParse(userIdClaim, out var userId) ? userId : (Guid?)null,
                    Action = action,
                    Entity = entity,
                    IPAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
                };

                await auditService.LogAsync(auditLog);
            }
        }
        else
        {
            await _next(context);
        }
    }
}

public static class AuditLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseAuditLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuditLoggingMiddleware>();
    }
}
