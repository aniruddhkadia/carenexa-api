using CarenexaApp.Application.MedicalRecords.Commands;
using CarenexaApp.Application.MedicalRecords.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarenexaApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MedicalRecordsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MedicalRecordsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Doctor,Nurse,Admin,SuperAdmin")]
    public async Task<IActionResult> GetPatientRecords(Guid patientId)
    {
        var result = await _mediator.Send(new GetPatientMedicalRecordsQuery(patientId));
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Doctor,Nurse,Admin,SuperAdmin")]
    public async Task<IActionResult> GetMedicalRecordById(Guid id)
    {
        var result = await _mediator.Send(new GetMedicalRecordByIdQuery(id));
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Doctor,Admin,SuperAdmin")]
    public async Task<IActionResult> CreateRecord([FromBody] CreateMedicalRecordCommand command)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var doctorId))
        {
            return BadRequest("DoctorId not found in token");
        }

        var cmdWithDoctor = command with { DoctorId = doctorId };
        var result = await _mediator.Send(cmdWithDoctor);
        
        return CreatedAtAction(nameof(GetMedicalRecordById), new { id = result }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Doctor,Admin,SuperAdmin")]
    public async Task<IActionResult> UpdateRecord(Guid id, [FromBody] UpdateMedicalRecordCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> DeleteRecord(Guid id)
    {
        var result = await _mediator.Send(new DeleteMedicalRecordCommand(id));
        if (!result) return NotFound();

        return NoContent();
    }
}
