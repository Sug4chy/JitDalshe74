using FluentValidation;
using JitDalshe.Ui.Admin.Utils;

namespace JitDalshe.Ui.Admin.Models.Forms;

public sealed class CreateEventFormModel
{
    public string? UploadedImageUrl { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Today;
    public bool IsDisplaying { get; set; }
}

public sealed class CreateEventFormModelValidator : AbstractValidator<CreateEventFormModel>
{
    public CreateEventFormModelValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage(ValidationMessages.TitleIsRequired);
        RuleFor(x => x.Date).GreaterThan(DateTime.Now).WithMessage(ValidationMessages.DateMustBeInFuture);
        RuleFor(x => x.UploadedImageUrl).NotEmpty().WithMessage(ValidationMessages.ImageIsRequired);
    }
}