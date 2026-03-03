namespace CarenexaApp.Domain.Enums;

public enum UserRole
{
    Doctor,
    Nurse,
    Staff,
    Admin,
    Insurance,
    SuperAdmin
}

public enum AppointmentStatus
{
    Booked,
    InProgress,
    Completed,
    Cancelled
}

public enum PaymentStatus
{
    Pending,
    Paid,
    PartiallyPaid,
    Overdue
}

public enum ClaimStatus
{
    Pending,
    Approved,
    Rejected
}

public enum AppointmentType
{
    Consultation,
    FollowUp,
    Procedure,
    Emergency
}

public enum MedicalRecordStatus
{
    Draft,
    Completed
}
