using System.Text.Json.Serialization;
using FluentValidation;
using JitDalshe.Api.Attributes;
using JitDalshe.Domain.Entities.Events;

namespace JitDalshe.Api.Admin.Controllers.Events.Requests;

[Validator<EditEventRequestValidator>]
public sealed record EditEventRequest(
    string Title,
    string ShortDescription,
    string FullText,
    DateOnly? Date,
    TimeOnly? Time,
    string? Location,
    [property: JsonConverter(typeof(JsonStringEnumConverter))] EventStatus Status
);

public sealed class EditEventRequestValidator : AbstractValidator<EditEventRequest>
{
    public EditEventRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ShortDescription).NotEmpty();
        RuleFor(x => x.FullText).NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
    }
}