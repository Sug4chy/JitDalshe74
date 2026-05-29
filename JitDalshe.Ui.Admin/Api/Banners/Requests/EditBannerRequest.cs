using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Api.Banners.Requests;

public sealed record EditBannerRequest(
    string? Title,
    string? Description,
    BannerStatus Status,
    bool IsClickable = false,
    string? RedirectOnClickUrl = null,
    int? DisplayOrder = null
);