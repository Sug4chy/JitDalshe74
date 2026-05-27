using System.Text.Json.Serialization;
using FluentValidation;
using JitDalshe.Api.Attributes;
using JitDalshe.Domain.Entities.Banners;

namespace JitDalshe.Api.Admin.Controllers.Banners.Requests;

[Validator<EditBannerRequestValidator>]
public sealed record EditBannerRequest(
    string? Title,
    string? Description,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    BannerStatus Status,
    bool IsClickable = false,
    string? RedirectOnClickUrl = null,
    int? DisplayOrder = null
);

public sealed class EditBannerRequestValidator : AbstractValidator<EditBannerRequest>
{
    public EditBannerRequestValidator()
    {
        RuleFor(x => x.RedirectOnClickUrl)
            .NotEmpty()
            .When(x => x.IsClickable);
        RuleFor(x => x.DisplayOrder)
            .Must(x => x is > 0 and < 5 or null);
    }
}