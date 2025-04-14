using System.Reflection;
using FluentValidation;
using JitDalshe.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JitDalshe.Api.ActionFilters;

public sealed class ValidateRequestActionFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidateRequestActionFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await ValidateRequestParams(context);
        if (context.Result is null)
            await next();
    }

    private async Task ValidateRequestParams(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments
                     .Where(arg => !context.RouteData.Values.ContainsKey(arg.Key)))
        {
            var type = argument.Value!.GetType();
            var validatorAttribute = type.GetCustomAttribute(typeof(ValidatorAttribute<>));
            if (validatorAttribute is null)
            {
                continue;
            }

            var validatorType = validatorAttribute.GetType().GetGenericArguments()[0];
            if (_serviceProvider.GetService(validatorType) is not IValidator validator)
            {
                context.Result = new ObjectResult(
                    new { Message = $"Cannot find validator for request type {type.Name}" })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                return;
            }

            
            var validationResult = await validator.ValidateAsync(new ValidationContext<object>(argument.Value));
            if (validationResult.IsValid)
            {
                continue;
            }
            
            context.Result = new BadRequestObjectResult(
                new ValidationProblemDetails(validationResult.ToDictionary())
            );
            return;
        }
    }
}