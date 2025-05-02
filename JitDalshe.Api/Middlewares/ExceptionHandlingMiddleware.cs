using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace JitDalshe.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;

    public ExceptionHandlingMiddleware(IEnumerable<IExceptionHandler> exceptionHandlers)
    {
        _exceptionHandlers = exceptionHandlers;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            foreach (var handler in _exceptionHandlers)
            {
                if (await handler.TryHandleAsync(context, e, context.RequestAborted))
                {
                    break;
                }
            }
        }
    }
}