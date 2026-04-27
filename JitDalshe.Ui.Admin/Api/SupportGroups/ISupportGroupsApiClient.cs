using JitDalshe.Ui.Admin.Api.SupportGroups.Requests;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Models.Common;
using Refit;

namespace JitDalshe.Ui.Admin.Api.SupportGroups;

public interface ISupportGroupsApiClient
{
    [Get("")]
    Task<ApiResponse<PagedResult<SupportGroupRequest>>> ListAsync(
        [Query] int pageNumber, 
        [Query] int pageSize, 
        [Query] RequestStatus? status,
        [Query] DateOnly? startDate,
        [Query] DateOnly? endDate,
        CancellationToken ct = default);

    [Patch("/{id}/status")]
    Task<ApiResponse<IApiResponse>> ChangeStatusAsync(Guid id, ChangeSupportGroupRequestStatusRequest request, CancellationToken ct = default); 

    [Patch("/{id}/comment")]
    Task<ApiResponse<IApiResponse>> UpdateCommentAsync(Guid id, UpdateSupportGroupRequestCommentRequest request, CancellationToken ct = default);
}