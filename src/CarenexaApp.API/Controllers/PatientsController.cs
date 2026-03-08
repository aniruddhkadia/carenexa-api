using AroviaApp.Application.Patients.Commands;
using AroviaApp.Application.Patients.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AroviaApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Doctor,Nurse,Staff,Admin,SuperAdmin")]
    public async Task<IActionResult> GetPatients([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("ClinicId not found in token");
        }

        var result = await _mediator.Send(new GetPatientsQuery(clinicId, page, pageSize, q));
        return Ok(result);
    }

    [HttpGet("search")]
    [Authorize(Roles = "Doctor,Nurse,Staff,Admin,SuperAdmin")]
    public async Task<IActionResult> SearchPatients([FromQuery] string q)
    {
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("ClinicId not found in token");
        }

        var result = await _mediator.Send(new SearchPatientsQuery(clinicId, q));
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Doctor,Nurse,Staff,Admin,SuperAdmin")]
    public async Task<IActionResult> GetPatientById(Guid id)
    {
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("ClinicId not found in token");
        }

        var result = await _mediator.Send(new GetPatientByIdQuery(id, clinicId));
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Doctor,Staff,Admin,SuperAdmin")]
    public async Task<IActionResult> CreatePatient([FromBody] CreatePatientCommand command)
    {
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("ClinicId not found in token");
        }

        var cmdWithClinic = command with { ClinicId = clinicId };
        
        try 
        {
            var result = await _mediator.Send(cmdWithClinic);
            return CreatedAtAction(nameof(GetPatientById), new { id = result }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Doctor,Staff,Admin,SuperAdmin")]
    public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientCommand command)
    {
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("ClinicId not found in token");
        }

        if (id != command.Id) return BadRequest("ID mismatch");

        var cmdWithClinic = command with { ClinicId = clinicId };
        var result = await _mediator.Send(cmdWithClinic);
        
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> DeletePatient(Guid id)
    {
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("ClinicId not found in token");
        }

        var result = await _mediator.Send(new DeletePatientCommand(id, clinicId));
        if (!result) return NotFound();

        return NoContent();
    }
}
