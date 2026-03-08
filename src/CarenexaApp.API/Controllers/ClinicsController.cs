using AroviaApp.Application.Clinics.Commands;
using AroviaApp.Application.Clinics.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AroviaApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClinicsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClinicsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("my-clinic")]
    [Authorize(Roles = "Admin,SuperAdmin,Doctor,Nurse,Staff")]
    public async Task<IActionResult> GetMyClinic()
    {
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("ClinicId not found in token");
        }

        var result = await _mediator.Send(new GetClinicQuery(clinicId));
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPut("my-clinic")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> UpdateMyClinic([FromBody] UpdateClinicCommand command)
    {
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("ClinicId not found in token");
        }

        if (clinicId != command.Id)
        {
            return BadRequest("Clinic ID mismatch");
        }

        var result = await _mediator.Send(command);
        if (!result) return NotFound();

        return NoContent();
    }
}
