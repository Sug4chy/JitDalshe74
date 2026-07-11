using JitDalshe.Ui.Admin.Api.Volunteers.Requests;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Models.Common;
using Refit;

namespace JitDalshe.Ui.Admin.Api.Volunteers;

public interface IVolunteersApiClient
{
    [Get("")]
    Task<IApiResponse<PagedResult<VolunteerRequest>>> ListAsync(
        [Query] int pageNumber, 
        [Query] int pageSize, 
        [Query] RequestStatus? status,
        [Query] DateOnly? startDate,
        [Query] DateOnly? endDate,
        CancellationToken ct = default);

    [Patch("/{id}/status")]
    Task<IApiResponse> ChangeStatusAsync(Guid id, ChangeVolunteerRequestStatusRequest request, CancellationToken ct = default); 

    [Patch("/{id}/comment")]
    Task<IApiResponse> UpdateCommentAsync(Guid id, UpdateVolunteerRequestCommentRequest request, CancellationToken ct = default);
}