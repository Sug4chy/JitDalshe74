using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Site.Controllers.Events.Requests;

[Validator<ListEventsRequestValidator>]
public sealed record ListEventsRequest(int PageNumber, int PageSize);

public sealed class ListEventsRequestValidator : AbstractValidator<ListEventsRequest>
{
    public ListEventsRequestValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}