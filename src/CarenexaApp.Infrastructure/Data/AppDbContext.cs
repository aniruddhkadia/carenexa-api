using CarenexaApp.Application.Common.Interfaces;
using CarenexaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Clinic> Clinics => Set<Clinic>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<Billing> Billings => Set<Billing>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<InsuranceClaim> InsuranceClaims => Set<InsuranceClaim>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Clinic - Users (One-to-Many)
        modelBuilder.Entity<Clinic>()
            .HasMany(c => c.Users)
            .WithOne(u => u.Clinic)
            .HasForeignKey(u => u.ClinicId);

        // Configure Clinic - Patients (One-to-Many)
        modelBuilder.Entity<Clinic>()
            .HasMany(c => c.Patients)
            .WithOne(p => p.Clinic)
            .HasForeignKey(p => p.ClinicId);

        // Configure Patient - Appointments (One-to-Many)
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Appointments)
            .WithOne(a => a.Patient)
            .HasForeignKey(a => a.PatientId);

        // Configure Patient - MedicalRecords (One-to-Many)
        modelBuilder.Entity<Patient>()
            .HasMany(p => p.MedicalRecords)
            .WithOne(m => m.Patient)
            .HasForeignKey(m => m.PatientId);

        // Disable cascade delete for some relationships to avoid cycles
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.NoAction;
        }
    }
}
