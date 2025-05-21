namespace JitDalshe.Application.Admin.Dto;

public readonly record struct BannerDto(
    Guid Id,
    string Title,
    string? RedirectOnClickUrl,
    int? DisplayOrder,
    string ImageUrl
);