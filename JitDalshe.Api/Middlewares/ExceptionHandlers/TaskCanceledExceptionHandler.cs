using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace JitDalshe.Api.Middlewares.ExceptionHandlers;

public sealed class TaskCanceledExceptionHandler : IExceptionHandler
{
    private readonly ILogger<TaskCanceledExceptionHandler> _logger;

    public TaskCanceledExceptionHandler(ILogger<TaskCanceledExceptionHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        if (exception is not TaskCanceledException)
        {
            return ValueTask.FromResult(false);
        }

        httpContext.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
        _logger.LogError(exception, "[{Type}]: [{Message}]", exception.GetType().Name, exception.Message);

        return ValueTask.FromResult(true);
    }
}