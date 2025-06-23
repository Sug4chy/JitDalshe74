namespace JitDalshe.Application.Admin.Dto;

public readonly record struct EventDto(
    Guid Id,
    string Title,
    string? Description,
    DateOnly Date,
    string ImageUrl,
    bool IsDisplaying
);