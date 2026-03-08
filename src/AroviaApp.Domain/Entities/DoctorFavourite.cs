namespace AroviaApp.Domain.Entities;

public class DoctorFavourite : BaseEntity
{
    public Guid DoctorId { get; set; }
    public Guid MedicineId { get; set; }

    // Navigation properties
    public User? Doctor { get; set; }
    public Medicine? Medicine { get; set; }
}
