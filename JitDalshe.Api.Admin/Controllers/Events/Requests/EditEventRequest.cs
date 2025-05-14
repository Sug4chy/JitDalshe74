using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.Events.Requests;

[Validator<EditEventRequestValidator>]
public sealed record EditEventRequest(
    string Title,
    string? Description,
    DateTime Date,
    string ImageBase64Url
);

public sealed class EditEventRequestValidator : AbstractValidator<EditEventRequest>
{
    public EditEventRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Date).GreaterThan(DateTime.Now);
        RuleFor(x => x.ImageBase64Url).NotEmpty();
    }
}