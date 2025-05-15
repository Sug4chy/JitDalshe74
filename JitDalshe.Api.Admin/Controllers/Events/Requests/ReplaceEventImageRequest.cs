using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.Events.Requests;

[Validator<ReplaceEventImageRequestValidator>]
public sealed record ReplaceEventImageRequest(
    string ImageBase64Url
);

public sealed class ReplaceEventImageRequestValidator : AbstractValidator<ReplaceEventImageRequest>
{
    public ReplaceEventImageRequestValidator()
    {
        RuleFor(x => x.ImageBase64Url).NotEmpty();
    }
}