namespace CarenexaApp.Application.MedicalRecords.Queries;

public record MedicineDto(
    Guid Id, 
    string GenericName, 
    string BrandName, 
    string Strength, 
    string Form,
    bool IsActive,
    DateTime CreatedAt);
