using AroviaApp.Application.Appointments.Commands;
using AroviaApp.Application.Appointments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AroviaApp.API.Controllers;

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

    [HttpGet("today")]
    [Authorize(Roles = "Doctor,Nurse,Staff,Admin,SuperAdmin")]
    public async Task<IActionResult> GetTodayAppointments()
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(doctorIdClaim, out var doctorId))
        {
            return BadRequest("DoctorId not found in token");
        }

        var result = await _mediator.Send(new GetAppointmentsQuery(doctorId, DateTime.UtcNow.Date));
        return Ok(result);
    }

    [HttpGet("calendar")]
    [Authorize(Roles = "Doctor,Nurse,Staff,Admin,SuperAdmin")]
    public async Task<IActionResult> GetCalendarAppointments([FromQuery] int month, [FromQuery] int year)
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(doctorIdClaim, out var doctorId))
        {
            return BadRequest("DoctorId not found in token");
        }
        
        if(month == 0 || year == 0) return BadRequest("Month and Year are required.");

        var result = await _mediator.Send(new GetMonthlyAppointmentsQuery(doctorId, year, month));
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Doctor,Nurse,Staff,Admin,SuperAdmin")]
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

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Doctor,Nurse,Staff,Admin,SuperAdmin")]
    public async Task<IActionResult> GetPatientAppointments(Guid patientId)
    {
        var result = await _mediator.Send(new GetPatientAppointmentsQuery(patientId));
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Doctor,Nurse,Staff,Admin,SuperAdmin")]
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
    [Authorize(Roles = "Doctor,Staff,Admin,SuperAdmin")]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;

        if (!Guid.TryParse(doctorIdClaim, out var doctorId) || !Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("DoctorId or ClinicId not found in token");
        }

        var cmdWithMetadata = command with { DoctorId = doctorId, ClinicId = clinicId };
        var result = await _mediator.Send(cmdWithMetadata);
        
        return CreatedAtAction(nameof(GetAppointmentById), new { id = result }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Doctor,Staff,Admin,SuperAdmin")]
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

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Doctor,Staff,Admin,SuperAdmin")]
    public async Task<IActionResult> UpdateAppointmentStatus(Guid id, [FromBody] AroviaApp.Domain.Enums.AppointmentStatus status)
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(doctorIdClaim, out var doctorId))
        {
            return BadRequest("DoctorId not found in token");
        }

        var result = await _mediator.Send(new UpdateAppointmentStatusCommand(id, doctorId, status));
        
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Doctor,Staff,Admin,SuperAdmin")]
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
