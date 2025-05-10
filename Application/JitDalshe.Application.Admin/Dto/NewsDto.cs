namespace JitDalshe.Application.Admin.Dto;

public readonly record struct NewsDto(
    Guid Id,
    string Text,
    DateOnly PublishDate,
    NewsImageDto[] Images
);