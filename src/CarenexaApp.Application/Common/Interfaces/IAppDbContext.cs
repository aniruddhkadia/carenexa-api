using CarenexaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<Clinic> Clinics { get; }
    DbSet<Patient> Patients { get; }
    DbSet<Appointment> Appointments { get; }
    DbSet<MedicalRecord> MedicalRecords { get; }
    DbSet<CarenexaApp.Domain.Entities.Billing> Billings { get; }
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<InsuranceClaim> InsuranceClaims { get; }
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
