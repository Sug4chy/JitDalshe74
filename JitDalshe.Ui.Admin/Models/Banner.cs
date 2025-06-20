namespace JitDalshe.Ui.Admin.Models;

public sealed record Banner(
    Guid Id,
    string Title,
    string ImageUrl,
    int? DisplayOrder,
    string? RedirectOnClickUrl = null
);