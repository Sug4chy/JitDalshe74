namespace JitDalshe.Ui.Admin.Api.Banners.Requests;

public sealed record CreateBannerRequest(
    string? Title,
    string? Description,
    string ImageBase64Url,
    string MobileImageBase64Url,
    bool IsClickable = false,
    string? RedirectOnClickUrl = null,
    int? DisplayOrder = null
);