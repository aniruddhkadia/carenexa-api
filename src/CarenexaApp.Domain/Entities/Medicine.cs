namespace CarenexaApp.Domain.Entities;

public class Medicine : BaseEntity
{
    public string GenericName { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public string Strength { get; set; } = string.Empty;
    public string Form { get; set; } = string.Empty; // Tablet, Syrup, etc.
    public bool IsActive { get; set; } = true;
}
