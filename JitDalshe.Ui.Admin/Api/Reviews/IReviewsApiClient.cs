using JitDalshe.Ui.Admin.Models;
using Refit;

namespace JitDalshe.Ui.Admin.Api.Reviews;

public interface IReviewsApiClient
{
    [Get("/unmoderated")]
    Task<IApiResponse<UnmoderatedReview[]>> ListUnmoderatedReviewsAsync();

    [Post("/{id}/moderate")]
    Task<IApiResponse> ModerateReviewAsync(Guid id);

    [Delete("/{id}")]
    Task<IApiResponse> DeleteReviewAsync(Guid id);
}