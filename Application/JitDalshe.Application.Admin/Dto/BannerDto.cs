using JitDalshe.Domain.Entities.Banners;

namespace JitDalshe.Application.Admin.Dto;

public readonly record struct BannerDto(
    Guid Id,
    string? Title,
    string? Description,
    string? RedirectOnClickUrl,
    int? DisplayOrder,
    BannerStatus Status,
    string ImageUrl,
    string MobileImageUrl
);