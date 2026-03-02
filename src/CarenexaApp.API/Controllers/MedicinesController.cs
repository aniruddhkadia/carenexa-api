using CarenexaApp.Application.MedicalRecords.Commands;
using CarenexaApp.Application.MedicalRecords.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarenexaApp.API.Controllers;

[Authorize(Roles = "Doctor,Nurse,Admin,SuperAdmin")]
[ApiController]
[Route("api/[controller]")]
public class MedicinesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MedicinesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchMedicines([FromQuery] string q)
    {
        var result = await _mediator.Send(new SearchMedicinesQuery(q));
        return Ok(result);
    }

    [HttpGet("favourites")]
    public async Task<IActionResult> GetFavourites()
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(doctorIdClaim, out var doctorId))
            return BadRequest("DoctorId not found in token");

        var result = await _mediator.Send(new GetDoctorFavouritesQuery(doctorId));
        return Ok(result);
    }

    [HttpPost("favourites")]
    public async Task<IActionResult> AddFavourite([FromBody] Guid medicineId)
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(doctorIdClaim, out var doctorId))
            return BadRequest("DoctorId not found in token");

        var result = await _mediator.Send(new AddDoctorFavouriteCommand(doctorId, medicineId));
        return Ok(result);
    }

    [HttpDelete("favourites/{id}")]
    public async Task<IActionResult> RemoveFavourite(Guid id)
    {
        var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(doctorIdClaim, out var doctorId))
            return BadRequest("DoctorId not found in token");

        var result = await _mediator.Send(new RemoveDoctorFavouriteCommand(doctorId, id));
        if (!result) return NotFound();
        return NoContent();
    }
}
