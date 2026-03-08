using AroviaApp.Application.Common.Interfaces;
using AroviaApp.Infrastructure.Data;
using AroviaApp.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AroviaApp.Infrastructure.Services;


namespace AroviaApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
        
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuditService, AuditService>();


        return services;
    }
}
