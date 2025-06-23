namespace JitDalshe.Ui.Admin.Api.Banners.Requests;

public sealed record CreateBannerRequest(
    string Title,
    string ImageBase64Url,
    bool IsClickable = false,
    string? RedirectOnClickUrl = null,
    int? DisplayOrder = null
);