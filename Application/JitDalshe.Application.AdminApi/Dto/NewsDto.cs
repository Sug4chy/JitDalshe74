namespace JitDalshe.Application.AdminApi.Dto;

public readonly record struct NewsDto(
    Guid Id,
    string Text,
    NewsPhotoDto[] Photos
);