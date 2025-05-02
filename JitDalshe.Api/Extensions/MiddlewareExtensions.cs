using JitDalshe.Api.Middlewares;
using JitDalshe.Api.Middlewares.ExceptionHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace JitDalshe.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        => services.AddExceptionHandler<GlobalExceptionHandler>()
            .AddExceptionHandler<TaskCanceledExceptionHandler>()
            .AddSingleton<ExceptionHandlingMiddleware>();

    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlingMiddleware>();
}