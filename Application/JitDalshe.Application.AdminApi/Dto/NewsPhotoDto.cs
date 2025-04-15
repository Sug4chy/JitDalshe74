namespace JitDalshe.Application.AdminApi.Dto;

public readonly record struct NewsPhotoDto(
    Guid Id,
    string Uri,
    bool IsPrimary
);