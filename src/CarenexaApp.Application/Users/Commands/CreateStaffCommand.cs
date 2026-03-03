using CarenexaApp.Application.Common.Interfaces;
using CarenexaApp.Domain.Entities;
using CarenexaApp.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.Users.Commands;

public record CreateStaffCommand : IRequest<Guid>
{
    public Guid ClinicId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Role { get; init; } = "Staff"; // Nurse | Staff | Doctor
    public string PhoneNumber { get; init; } = string.Empty;
    public string Specialization { get; init; } = string.Empty;
    public string Qualification { get; init; } = string.Empty;
}

public class CreateStaffCommandHandler : IRequestHandler<CreateStaffCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateStaffCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
        {
            throw new Exception("Email already registered");
        }

        if (!Enum.TryParse<UserRole>(request.Role, out var userRole))
        {
            userRole = UserRole.Staff;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = userRole,
            ClinicId = request.ClinicId,
            PhoneNumber = request.PhoneNumber,
            Specialization = request.Specialization,
            Qualification = request.Qualification,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
