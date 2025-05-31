using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.Banners.Requests;

[Validator<EditBannerRequestValidator>]
public sealed record EditBannerRequest(
    string Title,
    bool IsClickable = false,
    string? RedirectOnClickUrl = null,
    int? DisplayOrder = null
);

public sealed class EditBannerRequestValidator : AbstractValidator<EditBannerRequest>
{
    public EditBannerRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.IsClickable).NotNull();
        RuleFor(x => x.RedirectOnClickUrl)
            .NotNull()
            .When(x => x.IsClickable);
        RuleFor(x => x.DisplayOrder)
            .Must(x => x is > 0 and < 5 or null);
    }
}