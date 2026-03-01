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
