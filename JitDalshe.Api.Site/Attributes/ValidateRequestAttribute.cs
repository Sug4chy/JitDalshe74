using JitDalshe.Api.Site.ActionFilters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JitDalshe.Api.Site.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ValidateRequestAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        => new ValidateRequestActionFilter(serviceProvider);
}