namespace JitDalshe.Ui.Admin.Models;

public sealed record PreviewBanner(
    string Title,
    string ImageUrl,
    int DisplayOrder,
    string? RedirectOnClickUrl = null
);