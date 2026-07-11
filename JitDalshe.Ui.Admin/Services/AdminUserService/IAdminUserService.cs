using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Services.AdminUserService;

public interface IAdminUserService
{
    Task<PagedResult<AdminUser>?> ListAsync(int pageNumber, int pageSize, bool? isActive = null, CancellationToken ct = default);
    Task<bool> CreateAsync(string email, string password, CancellationToken ct = default);
    Task<bool> ToggleStatusAsync(Guid id, bool isActive, CancellationToken ct = default);
    
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}