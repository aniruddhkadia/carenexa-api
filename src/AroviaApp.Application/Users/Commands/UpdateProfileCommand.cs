using AroviaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.Users.Commands;

public record UpdateProfileCommand : IRequest<bool>
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Specialization { get; init; } = string.Empty;
    public string Qualification { get; init; } = string.Empty;
}

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
{
    private readonly IAppDbContext _context;

    public UpdateProfileCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (user == null) return false;

        user.FullName = request.FullName;
        user.PhoneNumber = request.PhoneNumber;
        user.Specialization = request.Specialization;
        user.Qualification = request.Qualification;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
