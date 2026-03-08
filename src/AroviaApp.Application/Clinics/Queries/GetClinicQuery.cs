using AroviaApp.Application.Common.Interfaces;
using AroviaApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Clinics.Queries;

public record GetClinicQuery(Guid Id) : IRequest<ClinicDto?>;

public class GetClinicQueryHandler : IRequestHandler<GetClinicQuery, ClinicDto?>
{
    private readonly IAppDbContext _context;

    public GetClinicQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ClinicDto?> Handle(GetClinicQuery request, CancellationToken cancellationToken)
    {
        var clinic = await _context.Clinics
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (clinic == null) return null;

        return new ClinicDto
        {
            Id = clinic.Id,
            ClinicName = clinic.ClinicName,
            Address = clinic.Address,
            City = clinic.City,
            State = clinic.State,
            Country = clinic.Country,
            Phone = clinic.Phone,
            Email = clinic.Email,
            LogoUrl = clinic.LogoUrl
        };
    }
}

public class ClinicDto
{
    public Guid Id { get; set; }
    public string ClinicName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
}
