using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JitDalshe.Api.Admin.Controllers.Auth.Requests;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Application.Admin.UseCases.Auth.ChangePassword;
using JitDalshe.Application.Admin.UseCases.Auth.Login;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.Auth;

[ApiController]
[Route("/api-admin/v1/[controller]")]
[AllowAnonymous]
public sealed class AuthController : AbstractController
{
    [HttpPost("login")]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] ILoginUseCase loginUseCase,
        CancellationToken ct = default)
    {
        var result = await loginUseCase.LoginAsync(request.Email, request.Password, ct);
        
        if (result.IsFailure)
        {
            return Error(result.Error);
        }
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(result.Value);
        var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value ?? "Standard";
        
        Response.Cookies.Append("authToken", result.Value, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddHours(12)
        });

        Response.Cookies.Append("userRole", role, new CookieOptions
        {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddHours(12)
        });
        
        return Ok();
    }
    
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("authToken");
        Response.Cookies.Delete("userRole");
        return Ok();
    }
    
    [HttpPost("change-password")]
    [Authorize]
    [ValidateRequest]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        [FromServices] IChangePasswordUseCase changePasswordUseCase,
        CancellationToken ct = default)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var result = await changePasswordUseCase.ChangeAsync(
            IdOf<AdminUser>.From(userId), 
            request.CurrentPassword, 
            request.NewPassword, 
            ct);

        return result.IsSuccess ? Ok() : Error(result.Error);
    }
}