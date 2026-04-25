using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Requests;

[Validator<ListWithPaginationRequestValidator>]
public sealed record ListWithPaginationRequest(int PageNumber, int PageSize, ConsultationRequestFilter? Filter = null);

public sealed class ListWithPaginationRequestValidator : AbstractValidator<ListWithPaginationRequest>
{
    public ListWithPaginationRequestValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).LessThanOrEqualTo(100);
    }
}
