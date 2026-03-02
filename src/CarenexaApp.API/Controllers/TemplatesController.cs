using CarenexaApp.Application.MedicalRecords.Commands;
using CarenexaApp.Application.MedicalRecords.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarenexaApp.API.Controllers;

[Authorize(Roles = "Doctor")]
[ApiController]
[Route("api/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TemplatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetTemplates()
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(doctorIdClaim, out var doctorId))
            return BadRequest("DoctorId not found in token");

        var result = await _mediator.Send(new GetDoctorTemplatesQuery(doctorId));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateVisitTemplateCommand command)
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(doctorIdClaim, out var doctorId))
            return BadRequest("DoctorId not found in token");

        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
            return BadRequest("ClinicId not found in token");

        var cmdWithDoctorId = command with { DoctorId = doctorId, ClinicId = clinicId };
        var result = await _mediator.Send(cmdWithDoctorId);
        
        return Ok(result);
    }
}
