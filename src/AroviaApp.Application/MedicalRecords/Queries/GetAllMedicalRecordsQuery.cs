using AroviaApp.Application.Common.Interfaces;
using AroviaApp.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AroviaApp.Application.MedicalRecords.Queries;

public record GetAllMedicalRecordsQuery(int Page = 1, int PageSize = 50) : IRequest<PaginatedResult<MedicalRecordListDto>>;

public record MedicalRecordListDto(
    Guid Id,
    string PatientFullName,
    string Diagnosis,
    string DoctorName,
    DateTime CreatedAt,
    string Status
);

public class GetAllMedicalRecordsQueryHandler : IRequestHandler<GetAllMedicalRecordsQuery, PaginatedResult<MedicalRecordListDto>>
{
    private readonly IAppDbContext _context;

    public GetAllMedicalRecordsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<MedicalRecordListDto>> Handle(GetAllMedicalRecordsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MedicalRecords
            .AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(m => m.Patient)
            .Include(m => m.Doctor)
            .OrderByDescending(m => m.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(m => new MedicalRecordListDto(
                m.Id,
                m.Patient!.FirstName + " " + m.Patient.LastName,
                m.Diagnosis,
                m.Doctor!.FullName,
                m.CreatedAt,
                m.Status.ToString()
            ))
            .ToListAsync(cancellationToken);

        return new PaginatedResult<MedicalRecordListDto>(items, totalCount, request.Page, request.PageSize);
    }
}
