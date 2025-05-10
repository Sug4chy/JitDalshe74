using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace JitDalshe.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddSwaggerGenWithControllerGroups(this IServiceCollection services)
        => services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "JitDalshe74 API", Version = "v1" });

            c.TagActionsBy(api =>
            {
                if (api.GroupName != null)
                {
                    return [api.GroupName];
                }

                if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    return [controllerActionDescriptor.ControllerName];
                }

                throw new InvalidOperationException("Unable to determine tag for endpoint.");
            });
            c.DocInclusionPredicate((_, _) => true);
        });
}