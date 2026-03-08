using AroviaApp.Application.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AroviaApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            if (result.IsInactive)
            {
                return StatusCode(403, new { message = result.ErrorMessage });
            }
            return Unauthorized(new { message = result.ErrorMessage });
        }

        return Ok(result.Response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);

        if (result == null)
        {
            return Unauthorized(new { message = "Invalid refresh token" });
        }

        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "If your email is registered, you will receive a reset link." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result)
        {
            return BadRequest(new { message = "Invalid request or expired token." });
        }

        return Ok(new { message = "Password reset successfully." });
    }
}
