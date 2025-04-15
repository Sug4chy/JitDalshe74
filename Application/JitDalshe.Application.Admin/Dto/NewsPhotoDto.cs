namespace JitDalshe.Application.Admin.Dto;

public readonly record struct NewsPhotoDto(
    Guid Id,
    string Uri,
    bool IsPrimary
);