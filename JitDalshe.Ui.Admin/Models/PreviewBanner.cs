namespace JitDalshe.Ui.Admin.Models;

public sealed record PreviewBanner(
    string? Title,
    string? Description,
    string ImageUrl,
    string MobileImageUrl,
    int DisplayOrder,
    string? RedirectOnClickUrl = null
);