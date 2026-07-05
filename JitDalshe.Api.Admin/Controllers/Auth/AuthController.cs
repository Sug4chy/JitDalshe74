using JitDalshe.Api.Admin.Controllers.Auth.Requests;
using JitDalshe.Api.Attributes;
using JitDalshe.Application.Admin.UseCases.Auth.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.Auth;

[ApiController]
[Route("/api-admin/v1/[controller]")]
[AllowAnonymous]
public sealed class AuthController : ControllerBase
{
    [HttpPost("login")]
    [ValidateRequest]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] ILoginUseCase loginUseCase,
        CancellationToken ct = default)
    {
        var result = await loginUseCase.LoginAsync(request.Email, request.Password, ct);

        return result.Match<IActionResult>(
            success => Ok(new { success.Token }),
            _ => Unauthorized(new { Message = "Неверный email или пароль" }),
            error => StatusCode(500, new { error.Message })
        );
    }
}