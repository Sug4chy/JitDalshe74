using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Site.Controllers.News.Requests;

[Validator<ListNewsRequestValidator>]
public sealed record ListNewsRequest(int PageNumber, int PageSize);

public sealed class ListNewsRequestValidator : AbstractValidator<ListNewsRequest>
{
    public ListNewsRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1);
    }
}