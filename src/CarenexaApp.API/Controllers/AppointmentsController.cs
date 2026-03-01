using CarenexaApp.Application.Appointments.Commands;
using CarenexaApp.Application.Appointments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarenexaApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "Doctor,Nurse,Admin,SuperAdmin")]
    public async Task<IActionResult> GetAppointments([FromQuery] DateTime? date)
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(doctorIdClaim, out var doctorId))
        {
            return BadRequest("DoctorId not found in token");
        }

        var result = await _mediator.Send(new GetAppointmentsQuery(doctorId, date));
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Doctor,Nurse,Admin,SuperAdmin")]
    public async Task<IActionResult> GetAppointmentById(Guid id)
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(doctorIdClaim, out var doctorId))
        {
            return BadRequest("DoctorId not found in token");
        }

        var result = await _mediator.Send(new GetAppointmentByIdQuery(id, doctorId));
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Doctor,Admin,SuperAdmin")]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(doctorIdClaim, out var doctorId))
        {
            return BadRequest("DoctorId not found in token");
        }

        var cmdWithDoctor = command with { DoctorId = doctorId };
        var result = await _mediator.Send(cmdWithDoctor);
        
        return CreatedAtAction(nameof(GetAppointmentById), new { id = result }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Doctor,Admin,SuperAdmin")]
    public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody] UpdateAppointmentCommand command)
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(doctorIdClaim, out var doctorId))
        {
            return BadRequest("DoctorId not found in token");
        }

        if (id != command.Id) return BadRequest("ID mismatch");

        var cmdWithDoctor = command with { DoctorId = doctorId };
        var result = await _mediator.Send(cmdWithDoctor);
        
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> DeleteAppointment(Guid id)
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(doctorIdClaim, out var doctorId))
        {
            return BadRequest("DoctorId not found in token");
        }

        var result = await _mediator.Send(new DeleteAppointmentCommand(id, doctorId));
        if (!result) return NotFound();

        return NoContent();
    }
}
