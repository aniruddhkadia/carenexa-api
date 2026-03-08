using AroviaApp.Application.Common.Interfaces;
using AroviaApp.Domain.Entities;
using MediatR;

namespace AroviaApp.Application.MedicalRecords.Commands;

public record CreateVisitTemplateCommand(
    Guid DoctorId, 
    Guid ClinicId,
    string TemplateName, 
    string Diagnosis, 
    string Advice, 
    string PrescriptionJson
) : IRequest<Guid>;

public class CreateVisitTemplateCommandHandler : IRequestHandler<CreateVisitTemplateCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateVisitTemplateCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateVisitTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = new VisitTemplate
        {
            DoctorId = request.DoctorId,
            ClinicId = request.ClinicId,
            TemplateName = request.TemplateName,
            Diagnosis = request.Diagnosis,
            Advice = request.Advice,
            PrescriptionJson = request.PrescriptionJson
        };

        _context.VisitTemplates.Add(template);
        await _context.SaveChangesAsync(cancellationToken);

        return template.Id;
    }
}
