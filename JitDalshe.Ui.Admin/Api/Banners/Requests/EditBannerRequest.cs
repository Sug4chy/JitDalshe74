namespace JitDalshe.Ui.Admin.Api.Banners.Requests;

public sealed record EditBannerRequest(
    string Title,
    bool IsClickable = false,
    string? RedirectOnClickUrl = null,
    int? DisplayOrder = null
);