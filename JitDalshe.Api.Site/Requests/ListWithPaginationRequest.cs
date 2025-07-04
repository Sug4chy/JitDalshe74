using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Site.Requests;


[Validator<ListWithPaginationRequestValidator>]
public sealed record ListWithPaginationRequest(int PageNumber, int PageSize);

public sealed class ListWithPaginationRequestValidator : AbstractValidator<ListWithPaginationRequest>
{
    public ListWithPaginationRequestValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}