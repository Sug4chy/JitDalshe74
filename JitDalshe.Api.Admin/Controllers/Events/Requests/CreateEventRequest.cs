using System.Text.Json.Serialization;
using FluentValidation;
using JitDalshe.Api.Attributes;
using JitDalshe.Domain.Entities.Events;

namespace JitDalshe.Api.Admin.Controllers.Events.Requests;

[Validator<CreateEventRequestValidator>]
public sealed record CreateEventRequest(
    string Title,
    string ShortDescription,
    string FullText,
    DateOnly? Date,
    TimeOnly? Time,
    string? Location,
    [property: JsonConverter(typeof(JsonStringEnumConverter))] EventStatus Status,
    string ImageBase64Url
);

public sealed class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.ShortDescription).NotEmpty().WithMessage("Short description is required");
        RuleFor(x => x.FullText).NotEmpty().WithMessage("Full text is required");
        RuleFor(x => x.ImageBase64Url).NotEmpty().WithMessage("ImageBase64Url is required");
        RuleFor(x => x.Status).IsInEnum().WithMessage("Status is invalid");
        RuleFor(x => x)
            .Must(e =>
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                
                if (!e.Date.HasValue) return true;
                if (e.Date.Value < today) return false;
                if (e.Date.Value == today && e.Time.HasValue)
                {
                    var nowTime = TimeOnly.FromDateTime(DateTime.Now);
                    if (e.Time.Value <= nowTime) return false;
                }

                return true;
            }).WithMessage("Событие должно быть в будущем");
    }
}