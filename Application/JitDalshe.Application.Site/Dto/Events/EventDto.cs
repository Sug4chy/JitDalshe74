namespace JitDalshe.Application.Site.Dto.Events;

public readonly record struct EventDto (
    Guid Id,
    string Title,
    string ShortDescription,
    string FullText,
    DateOnly? Date,
    TimeOnly? Time,
    string? Location,
    string ImageUrl
);