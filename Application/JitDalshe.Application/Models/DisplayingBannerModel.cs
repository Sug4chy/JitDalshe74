namespace JitDalshe.Application.Models;

public sealed record DisplayingBannerModel(
    string Title,
    string ImageUrl,
    int DisplayOrder,
    string? RedirectOnClickUrl = null
);