using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JitDalshe.Api.Filters;

public sealed class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo.DeclaringType != null &&
                           (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                            context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any());

        if (!hasAuthorize)
        {
            return;
        }

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            }
        };

        if (!operation.Responses.ContainsKey(StatusCodes.Status401Unauthorized.ToString()))
        {
            operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new OpenApiResponse { Description = "Токен отсутствует или невалиден" });
        }

        if (!operation.Responses.ContainsKey(StatusCodes.Status403Forbidden.ToString()))
        {
            operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new OpenApiResponse { Description = "Недостаточно прав/роли" });
        }
    }
}