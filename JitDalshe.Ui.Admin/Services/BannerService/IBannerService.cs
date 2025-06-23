using JitDalshe.Ui.Admin.Api.Banners.Requests;
using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Services.BannerService;

public interface IBannerService
{
    Task<PreviewBanner[]> FindPreviewBannersAsync();
    Task<Banner[]> FindAllAsync();
    Task<bool> CreateBannerAsync(CreateBannerRequest request);
}