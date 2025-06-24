using JitDalshe.Ui.Admin.Api.Banners.Requests;
using JitDalshe.Ui.Admin.Models;
using Refit;

namespace JitDalshe.Ui.Admin.Api.Banners;

public interface IBannersApiClient
{
    [Get("/preview")]
    Task<IApiResponse<PreviewBanner[]>> GetPreviewBannersAsync();

    [Get("")]
    Task<IApiResponse<Banner[]>> ListBannersAsync();

    [Post("")]
    Task<IApiResponse> CreateBannerAsync([Body] CreateBannerRequest request);

    [Patch("/{id}")]
    Task<IApiResponse> EditBannerAsync(Guid id, [Body] EditBannerRequest request);

    [Patch("/{id}/image")]
    Task<IApiResponse> ReplaceBannerImageAsync(Guid id, [Body] ReplaceBannerImageRequest request);

    [Delete("/{id}")]
    Task<IApiResponse> DeleteBannerAsync(Guid id);
}