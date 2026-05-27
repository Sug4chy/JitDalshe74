namespace JitDalshe.Application.Models;

public sealed record DisplayingBannerModel(
    string? Title,
    string? Description,
    string ImageUrl,
    string MobileImageUrl,
    int DisplayOrder,
    string? RedirectOnClickUrl = null
);