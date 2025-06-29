using FluentValidation;
using JitDalshe.Ui.Admin.Utils;

namespace JitDalshe.Ui.Admin.Models.Forms;

public sealed class CreateBannerFormModel
{
    public string Title { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsDisplaying { get; set; }
    public int DisplayingOrder { get; set; }
    public bool IsClickable { get; set; }
    public string RedirectOnClickUrl { get; set; } = string.Empty;
}

public sealed class CreateBannerRequestValidator : AbstractValidator<CreateBannerFormModel>
{
    public CreateBannerRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage(ValidationMessages.TitleIsRequired);
        RuleFor(x => x.ImageUrl).NotEmpty().WithMessage(ValidationMessages.ImageIsRequired);
        RuleFor(x => x.RedirectOnClickUrl)
            .NotEmpty()
            .WithMessage(ValidationMessages.RedirectUrlRequired)
            .When(x => x.IsClickable);
        RuleFor(x => x.DisplayingOrder)
            .GreaterThan(0)
            .WithMessage(string.Format(ValidationMessages.NumberMustBeInRange, "Порядок отображения", "1", "5"))
            .LessThanOrEqualTo(5)
            .WithMessage(string.Format(ValidationMessages.NumberMustBeInRange, "Порядок отображения", "1", "5"))
            .When(x => x.IsDisplaying);
    }
}