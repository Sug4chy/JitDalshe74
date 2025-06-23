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
}