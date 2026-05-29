using JitDalshe.Ui.Admin.Api.Reviews.Requests;
using JitDalshe.Ui.Admin.Models;
using Refit;

namespace JitDalshe.Ui.Admin.Api.Reviews;

public interface IReviewsApiClient
{
    [Get("")]
    Task<IApiResponse<PagedResult<Review>>> ListReviewsAsync(
        [Query] int pageNumber,
        [Query] int pageSize,
        [Query] ReviewStatus? status,
        CancellationToken ct = default);

    [Patch("/{id}/status")]
    Task<IApiResponse> ChangeReviewStatusAsync(
        Guid id, 
        [Body] ChangeReviewStatusRequest request, 
        CancellationToken ct = default);

    [Delete("/{id}")]
    Task<IApiResponse> DeleteReviewAsync(Guid id);
}