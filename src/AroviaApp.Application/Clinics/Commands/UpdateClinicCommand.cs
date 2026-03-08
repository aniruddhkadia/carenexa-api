using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Clinics.Commands;

public record UpdateClinicCommand : IRequest<bool>
{
    public Guid Id { get; init; }
    public string ClinicName { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string LogoUrl { get; init; } = string.Empty;
}

public class UpdateClinicCommandHandler : IRequestHandler<UpdateClinicCommand, bool>
{
    private readonly IAppDbContext _context;

    public UpdateClinicCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateClinicCommand request, CancellationToken cancellationToken)
    {
        var clinic = await _context.Clinics
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (clinic == null) return false;

        clinic.ClinicName = request.ClinicName;
        clinic.Address = request.Address;
        clinic.City = request.City;
        clinic.State = request.State;
        clinic.Country = request.Country;
        clinic.Phone = request.Phone;
        clinic.Email = request.Email;
        clinic.LogoUrl = request.LogoUrl;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
