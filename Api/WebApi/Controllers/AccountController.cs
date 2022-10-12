using Application.DTOs.Account;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
    {
        return Ok(await _accountService.AuthenticateAsync(request, GenerateIPAddress()));
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        var origin = Request.Headers["origin"];
        return Ok(await _accountService.RegisterAsync(request, origin));
    }

    [HttpGet("profile")]
    [OpenApiOperation("Get profile details of current logged in users.", "")]
    public async Task<IActionResult> GetProfileAsync(CancellationToken cancellationToken)
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier) is not { } userId || string.IsNullOrEmpty(userId)
            ? Unauthorized()
            : Ok(await _accountService.GetAsync(userId, cancellationToken));
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
    {
        var origin = Request.Headers["origin"];
        return Ok(await _accountService.ConfirmEmailAsync(userId, code));
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
    {
        await _accountService.ForgotPassword(model, Request.Headers["origin"]);
        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
    {
        return Ok(await _accountService.ResetPassword(model));
    }

    private string GenerateIPAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        else
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }
}