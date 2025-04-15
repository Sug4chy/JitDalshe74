using JitDalshe.Api.ActionFilters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JitDalshe.Api.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ValidateRequestAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        => new ValidateRequestActionFilter(serviceProvider);
}