using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.Banners.Requests;

[Validator<ReplaceBannerImageRequestValidator>]
public sealed record ReplaceBannerImageRequest(
    string ImageBase64Url
);

public sealed class ReplaceBannerImageRequestValidator : AbstractValidator<ReplaceBannerImageRequest>
{
    public ReplaceBannerImageRequestValidator()
    {
        RuleFor(x => x.ImageBase64Url).NotEmpty();
    }
}