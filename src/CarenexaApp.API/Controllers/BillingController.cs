using CarenexaApp.Application.Billing.Commands;
using CarenexaApp.Application.Billing.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarenexaApp.API.Controllers;

[Authorize(Roles = "Admin,SuperAdmin")]
[ApiController]
[Route("api/[controller]")]
public class BillingController : ControllerBase
{
    private readonly IMediator _mediator;

    public BillingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetPatientBillings(Guid patientId)
    {
        var result = await _mediator.Send(new GetPatientBillingsQuery(patientId));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBillingById(Guid id)
    {
        var result = await _mediator.Send(new GetBillingByIdQuery(id));
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBillingById), new { id = result }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBilling(Guid id, [FromBody] UpdateBillingCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);
        if (!result) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBilling(Guid id)
    {
        var result = await _mediator.Send(new DeleteBillingCommand(id));
        if (!result) return NotFound();

        return NoContent();
    }
}
