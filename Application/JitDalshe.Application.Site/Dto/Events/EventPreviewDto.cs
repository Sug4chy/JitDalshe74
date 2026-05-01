namespace JitDalshe.Application.Site.Dto.Events;

public readonly record struct EventPreviewDto(
    Guid Id,
    string ShortDescription,
    string ImageUrl,
    DateOnly? Date
);