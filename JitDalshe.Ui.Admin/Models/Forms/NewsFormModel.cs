using FluentValidation;

namespace JitDalshe.Ui.Admin.Models.Forms;

public sealed class NewsFormModel()
{
    public Ref<Guid>? SelectedPhotoId { get; set; }
    public string CurrentEnteredText { get; set; } = null!;
    public bool IsDisplaying { get; set; }

    public NewsFormModel(Ref<Guid>? selectedPhotoId, string currentEnteredText, bool isDisplaying) : this()
    {
        SelectedPhotoId = selectedPhotoId;
        CurrentEnteredText = currentEnteredText;
        IsDisplaying = isDisplaying;
    }
}

public sealed class NewsFormModelValidator : AbstractValidator<NewsFormModel>
{
    public NewsFormModelValidator()
    {
        RuleFor(x => x.CurrentEnteredText)
            .NotEmpty()
            .WithMessage(ValidationMessages.TextIsRequired);
    }
}