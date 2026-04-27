using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Models.Common;

namespace JitDalshe.Ui.Admin.Services.SupportGroupService;

public interface ISupportGroupService
{
    Task<PagedResult<SupportGroupRequest>?> ListAsync(
        int pageNumber, 
        int pageSize, 
        RequestStatus? status = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        CancellationToken ct = default);
    Task<bool> ChangeStatusAsync(Guid id, RequestStatus status, CancellationToken ct = default);
    Task<bool> UpdateCommentAsync(Guid id, string? comment, CancellationToken ct = default);
}