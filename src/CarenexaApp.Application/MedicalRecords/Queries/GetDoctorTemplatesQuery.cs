using CarenexaApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarenexaApp.Application.MedicalRecords.Queries;

public record GetDoctorTemplatesQuery(Guid DoctorId) : IRequest<List<VisitTemplateDto>>;

public record VisitTemplateDto(
    Guid Id,
    string TemplateName,
    string Diagnosis,
    string Advice,
    string PrescriptionJson
);

public class GetDoctorTemplatesQueryHandler : IRequestHandler<GetDoctorTemplatesQuery, List<VisitTemplateDto>>
{
    private readonly IAppDbContext _context;

    public GetDoctorTemplatesQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<VisitTemplateDto>> Handle(GetDoctorTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await _context.VisitTemplates
            .Where(t => t.DoctorId == request.DoctorId)
            .Select(t => new VisitTemplateDto(t.Id, t.TemplateName, t.Diagnosis, t.Advice, t.PrescriptionJson))
            .ToListAsync(cancellationToken);
    }
}
