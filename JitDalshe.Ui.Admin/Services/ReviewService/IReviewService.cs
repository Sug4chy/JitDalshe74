using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Services.ReviewService;

public interface IReviewService
{
    Task<UnmoderatedReview[]> FindAllUnmoderatedReviewsAsync();
    Task ModerateReviewAsync(Guid reviewId, Func<Task>? onSuccess = null);
    Task DeleteReviewAsync(Guid reviewId, Func<Task>? onSuccess = null);
}