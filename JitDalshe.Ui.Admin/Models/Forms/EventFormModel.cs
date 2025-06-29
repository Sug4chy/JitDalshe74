using FluentValidation;
using JetBrains.Annotations;

namespace JitDalshe.Ui.Admin.Models.Forms;

public sealed class EventFormModel()
{
    public string? UploadedImageUrl { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Today;
    public bool IsDisplaying { get; set; }

    public EventFormModel(
        string? uploadedImageUrl, 
        string title,
        string description,
        DateTime date,
        bool isDisplaying) : this()
    {
        UploadedImageUrl = uploadedImageUrl;
        Title = title;
        Description = description;
        Date = date;
        IsDisplaying = isDisplaying;
    }
}

[UsedImplicitly]
public sealed class EventFormModelValidator : AbstractValidator<EventFormModel>
{
    public EventFormModelValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage(ValidationMessages.TitleIsRequired);
        RuleFor(x => x.Date).GreaterThan(DateTime.Now).WithMessage(ValidationMessages.DateMustBeInFuture);
        RuleFor(x => x.UploadedImageUrl).NotEmpty().WithMessage(ValidationMessages.ImageIsRequired);
    }
}