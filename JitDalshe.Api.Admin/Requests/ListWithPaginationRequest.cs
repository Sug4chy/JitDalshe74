using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Requests;

[Validator<ListWithPaginationRequestValidator>]
public sealed record ListWithPaginationRequest(int PageNumber, int PageSize);

public sealed class ListWithPaginationRequestValidator : AbstractValidator<ListWithPaginationRequest>
{
    public ListWithPaginationRequestValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).LessThanOrEqualTo(100);
    }
}
