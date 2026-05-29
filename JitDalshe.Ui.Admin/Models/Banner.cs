namespace JitDalshe.Ui.Admin.Models;

public sealed record Banner(
    Guid Id,
    string? Title,
    string? Description,
    string ImageUrl,
    string MobileImageUrl,
    int? DisplayOrder,
    BannerStatus Status,
    string? RedirectOnClickUrl = null
);

public enum BannerStatus
{
    NotPublished,
    Published
}