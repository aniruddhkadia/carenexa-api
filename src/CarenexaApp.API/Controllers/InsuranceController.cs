using AroviaApp.Application.Insurance.Commands;
using AroviaApp.Application.Insurance.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AroviaApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InsuranceController : ControllerBase
{
    private readonly IMediator _mediator;

    public InsuranceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Roles = "Doctor,Nurse,Admin,Insurance,SuperAdmin")]
    public async Task<IActionResult> GetPatientClaims(Guid patientId)
    {
        var result = await _mediator.Send(new GetInsuranceClaimsQuery(patientId));
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Doctor,Nurse,Admin,Insurance,SuperAdmin")]
    public async Task<IActionResult> GetClaimById(Guid id)
    {
        var result = await _mediator.Send(new GetInsuranceClaimByIdQuery(id));
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost("submit")]
    [Authorize(Roles = "Doctor,Admin,SuperAdmin")]
    public async Task<IActionResult> SubmitClaim([FromBody] SubmitClaimCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetClaimById), new { id = result }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Insurance,Admin,SuperAdmin")]
    public async Task<IActionResult> UpdateClaim(Guid id, [FromBody] UpdateInsuranceClaimCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> DeleteClaim(Guid id)
    {
        var result = await _mediator.Send(new DeleteInsuranceClaimCommand(id));
        if (!result) return NotFound();

        return NoContent();
    }
}
