namespace JitDalshe.Ui.Admin.Models;

public sealed record DisplayingBanner(
    string Title,
    string ImageUrl,
    int DisplayOrder,
    string? RedirectOnClickUrl = null
);