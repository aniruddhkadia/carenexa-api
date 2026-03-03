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
    
    // Phase 4 & 5
    public DbSet<Medicine> Medicines => Set<Medicine>();
    public DbSet<DoctorFavourite> DoctorFavourites => Set<DoctorFavourite>();
    public DbSet<VisitTemplate> VisitTemplates => Set<VisitTemplate>();

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

        // Configure Clinic - MedicalRecords (One-to-Many)
        modelBuilder.Entity<Clinic>()
            .HasMany(c => c.MedicalRecords)
            .WithOne(m => m.Clinic)
            .HasForeignKey(m => m.ClinicId);

        // Configure Clinic - VisitTemplates (One-to-Many)
        modelBuilder.Entity<Clinic>()
            .HasMany(c => c.VisitTemplates)
            .WithOne(v => v.Clinic)
            .HasForeignKey(v => v.ClinicId);

        // Configure MedicalRecord - Appointment (One-to-One / One-to-Many)
        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Appointment)
            .WithMany()
            .HasForeignKey(m => m.AppointmentId)
            .IsRequired(false);

        // Disable cascade delete for some relationships to avoid cycles
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.NoAction;
        }

        // Seed Medicines
        var med1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var med2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var med3Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var med4Id = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var med5Id = Guid.Parse("55555555-5555-5555-5555-555555555555");
        var med6Id = Guid.Parse("66666666-6666-6666-6666-666666666666");

        modelBuilder.Entity<Medicine>().HasData(
            new Medicine { Id = med1Id, GenericName = "Paracetamol", BrandName = "Dolo", Strength = "650mg", Form = "Tablet", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Medicine { Id = med2Id, GenericName = "Amoxicillin", BrandName = "Amoxil", Strength = "500mg", Form = "Capsule", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Medicine { Id = med3Id, GenericName = "Ibuprofen", BrandName = "Brufen", Strength = "400mg", Form = "Tablet", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Medicine { Id = med4Id, GenericName = "Cetirizine", BrandName = "Zyrtec", Strength = "10mg", Form = "Tablet", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Medicine { Id = med5Id, GenericName = "Pantoprazole", BrandName = "Pan 40", Strength = "40mg", Form = "Tablet", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Medicine { Id = med6Id, GenericName = "Azithromycin", BrandName = "Azee", Strength = "500mg", Form = "Tablet", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
