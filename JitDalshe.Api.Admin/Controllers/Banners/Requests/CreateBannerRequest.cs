using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.Banners.Requests;

[Validator<CreateBannerRequestValidator>]
public sealed record CreateBannerRequest(
    string Title,
    string ImageBase64Url,
    bool IsClickable = false,
    string? RedirectOnClickUrl = null,
    int? DisplayOrder = null
);

public sealed class CreateBannerRequestValidator : AbstractValidator<CreateBannerRequest>
{
    public CreateBannerRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.ImageBase64Url).NotEmpty();
        RuleFor(x => x.IsClickable).NotNull();
        RuleFor(x => x.RedirectOnClickUrl)
            .NotNull()
            .When(x => x.IsClickable);
        RuleFor(x => x.DisplayOrder)
            .Must(x => x is > 0 and < 5 or null);
    }
}