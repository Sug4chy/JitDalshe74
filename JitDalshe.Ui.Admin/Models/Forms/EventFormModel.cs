using FluentValidation;
using JetBrains.Annotations;

namespace JitDalshe.Ui.Admin.Models.Forms;

public sealed class EventFormModel()
{
    public string? UploadedImageUrl { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string FullText { get; set; } = string.Empty;
    public DateOnly? Date { get; set; }
    public TimeOnly? Time { get; set; }
    public string? Location { get; set; }
    public bool IsPublished { get; set; }
    

    public EventFormModel(Event ev) : this()
    {
        UploadedImageUrl = ev.ImageUrl;
        Title = ev.Title;
        ShortDescription = ev.ShortDescription;
        FullText = ev.FullText;
        Date = ev.Date;
        Time = ev.Time;
        Location = ev.Location;
        IsPublished = ev.IsDisplaying;
    } 
}

[UsedImplicitly]
public sealed class EventFormModelValidator : AbstractValidator<EventFormModel>
{
    public EventFormModelValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage(ValidationMessages.FieldIsRequired);
        RuleFor(x => x.ShortDescription).NotEmpty().WithMessage(ValidationMessages.FieldIsRequired);
        RuleFor(x => x.FullText).NotEmpty().WithMessage(ValidationMessages.FieldIsRequired);
        RuleFor(x => x.UploadedImageUrl).NotEmpty().WithMessage(ValidationMessages.ImageIsRequired);
        RuleFor(x => x)
            .Must(e =>
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                
                if (!e.Date.HasValue) return true;
                if (e.Date.Value < today) return false;
                if (e.Date.Value == today && e.Time.HasValue)
                {
                    var nowTime = TimeOnly.FromDateTime(DateTime.Now);
                    if (e.Time.Value <= nowTime) return false;
                }

                return true;
            }).WithMessage(ValidationMessages.DateMustBeInFuture);
    }
}