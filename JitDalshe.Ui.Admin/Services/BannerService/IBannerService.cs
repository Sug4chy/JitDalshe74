using JitDalshe.Ui.Admin.Api.Banners.Requests;
using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Services.BannerService;

public interface IBannerService
{
    Task<PreviewBanner[]> FindPreviewBannersAsync();
    Task<Banner[]> FindAllAsync();
    Task CreateBannerAsync(CreateBannerRequest request, Func<Task>? onSuccess = null);
    Task EditBannerAsync(Guid id, EditBannerRequest request, Func<Task>? onSuccess = null);
    Task ReplaceBannerImageAsync(Guid id, ReplaceBannerImageRequest request, Func<Task>? onSuccess = null);
    Task DeleteBannerAsync(Guid id, Func<Task>? onSuccess = null);
}