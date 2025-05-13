using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.Events.Requests;

[Validator<CreateEventRequestValidator>]
public sealed record CreateEventRequest(
    string Title,
    string? Description,
    DateTime Date,
    string ImageBase64Url
);

public sealed class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Date).GreaterThan(DateTime.Now);
        RuleFor(x => x.ImageBase64Url).NotEmpty();
    }
}