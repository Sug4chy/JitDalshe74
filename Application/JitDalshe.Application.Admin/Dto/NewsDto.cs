namespace JitDalshe.Application.Admin.Dto;

public readonly record struct NewsDto(
    Guid Id,
    string Text,
    NewsPhotoDto[] Photos
);