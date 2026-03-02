using CarenexaApp.Application.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarenexaApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] DateTime? date)
    {
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("ClinicId not found in token");
        }

        var result = await _mediator.Send(new GetDashboardSummaryQuery(clinicId, date));
        return Ok(result);
    }

    [HttpGet("activity")]
    public async Task<IActionResult> GetActivity()
    {
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("ClinicId not found in token");
        }

        var result = await _mediator.Send(new GetRecentActivityQuery(clinicId));
        return Ok(result);
    }

    [HttpGet("completed-visits")]
    public async Task<IActionResult> GetCompletedVisits([FromQuery] DateTime date)
    {
        var clinicIdClaim = User.FindFirst("ClinicId")?.Value;
        if (!Guid.TryParse(clinicIdClaim, out var clinicId))
        {
            return BadRequest("ClinicId not found in token");
        }

        var result = await _mediator.Send(new GetDailyCompletedVisitsQuery(clinicId, date));
        return Ok(result);
    }
}
