using FluentValidation;
using JitDalshe.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Site.Requests;


[Validator<ListWithPaginationRequestValidator>]
public sealed record ListWithPaginationRequest(
    [FromQuery(Name = "offset")] int Offset = 0, 
    [FromQuery(Name = "limit")] int Limit = 10
) {
    public int PageNumber => Offset + 1;
    public int PageSize => Limit;
}

public sealed class ListWithPaginationRequestValidator : AbstractValidator<ListWithPaginationRequest>
{
    public ListWithPaginationRequestValidator()
    {
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Limit).GreaterThan(0).LessThanOrEqualTo(100);
    }
}