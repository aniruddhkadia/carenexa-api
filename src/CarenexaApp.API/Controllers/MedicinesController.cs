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
<<<<<<< HEAD
    public async Task<IActionResult> SearchMedicines([FromQuery] string? q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            var allMedicines = await _mediator.Send(new GetMedicinesQuery());
            return Ok(allMedicines);
        }

=======
    public async Task<IActionResult> SearchMedicines([FromQuery] string q)
    {
>>>>>>> 6829967ddade774c1ea73506d65fb9d746b4b00c
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
<<<<<<< HEAD

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> CreateMedicine([FromBody] CreateMedicineCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> UpdateMedicine(Guid id, [FromBody] UpdateMedicineCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> DeleteMedicine(Guid id)
    {
        var result = await _mediator.Send(new DeleteMedicineCommand(id));
        if (!result) return NotFound();
        return NoContent();
    }
=======
>>>>>>> 6829967ddade774c1ea73506d65fb9d746b4b00c
}
