namespace JitDalshe.Application.Models;

public sealed record BannerModel(
    string Title,
    string ImageUrl,
    int DisplayOrder,
    string? RedirectOnClickUrl = null
);