using System.Security.Claims;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace JitDalshe.Api.Middlewares;

public sealed class ActiveAdminUserMiddleware(IAdminUsersRepository repository) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var identity = context.User.Identity;
        
        if (identity is { IsAuthenticated: true })
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                var maybeUser = await repository.FindByIdAsync(IdOf<AdminUser>.From(userId), context.RequestAborted);
                
                if (maybeUser.HasNoValue || !maybeUser.Value.IsActive)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new { Message = "Доступ заблокирован или учетная запись удалена." });
                    return;
                }
            }
        }

        await next(context);
    }
}