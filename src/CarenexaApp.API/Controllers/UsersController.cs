using CarenexaApp.Application.Users.Commands;
using CarenexaApp.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarenexaApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirst("Id")?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return BadRequest("User ID not found in token");
        }

        var result = await _mediator.Send(new GetProfileQuery(userId));
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        var userIdClaim = User.FindFirst("Id")?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return BadRequest("User ID not found in token");
        }

        if (userId != command.Id)
        {
            return BadRequest("User ID mismatch");
        }

        var result = await _mediator.Send(command);
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpGet("staff")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> GetStaff()
    {
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("ClinicId not found in token");
        }

        var result = await _mediator.Send(new GetStaffQuery(clinicId));
        return Ok(result);
    }

    [HttpPost("staff")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> CreateStaff([FromBody] CreateStaffCommand command)
    {
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("ClinicId not found in token");
        }

        var cmdWithClinic = command with { ClinicId = clinicId };
        var result = await _mediator.Send(cmdWithClinic);
        
        return Ok(new { id = result });
    }
}
