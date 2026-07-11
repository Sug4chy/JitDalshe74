using JitDalshe.Ui.Admin.Api.AdminUsers.Requests;
using JitDalshe.Ui.Admin.Models;
using Refit;

namespace JitDalshe.Ui.Admin.Api.AdminUsers;


public interface IAdminUsersApiClient
{
    [Get("")]
    Task<IApiResponse<PagedResult<AdminUser>>> ListAdminUsersAsync(
        [Query] int pageNumber, 
        [Query] int pageSize, 
        [Query] bool? isActive,
        CancellationToken ct = default);
    
    [Post("")]
    Task<IApiResponse> CreateAdminUserAsync(
        [Body] CreateAdminRequest request, 
        CancellationToken ct = default);
    
    [Patch("/{id}/active")]
    Task<IApiResponse> ChangeAdminUserActiveStatusAsync(
        Guid id, 
        [Body] ChangeAdminStatusRequest request, 
        CancellationToken ct = default);
    
    [Delete("/{id}")]
    Task<IApiResponse> DeleteAdminUserAsync(Guid id, CancellationToken ct = default);
}