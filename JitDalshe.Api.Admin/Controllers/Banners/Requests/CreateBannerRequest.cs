using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.Banners.Requests;

[Validator<CreateBannerRequestValidator>]
public sealed record CreateBannerRequest(
    string? Title,
    string? Description,
    string ImageBase64Url,
    string MobileImageBase64Url,
    bool IsClickable = false,
    string? RedirectOnClickUrl = null,
    int? DisplayOrder = null
);

public sealed class CreateBannerRequestValidator : AbstractValidator<CreateBannerRequest>
{
    public CreateBannerRequestValidator()
    {
        RuleFor(x => x.ImageBase64Url).NotEmpty();
        RuleFor(x => x.MobileImageBase64Url).NotEmpty();
        RuleFor(x => x.RedirectOnClickUrl)
            .NotEmpty()
            .When(x => x.IsClickable);
        RuleFor(x => x.DisplayOrder)
            .GreaterThan(0)
            .LessThanOrEqualTo(5)
            .When(x => x.DisplayOrder is not null);
    }
}