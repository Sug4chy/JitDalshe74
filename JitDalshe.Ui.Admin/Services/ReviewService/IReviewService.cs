using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Services.ReviewService;

public interface IReviewService
{
    Task<PagedResult<Review>?> ListAsync(
        int pageNumber, 
        int pageSize, 
        ReviewStatus? status = null, 
        CancellationToken ct = default);
        
    Task<bool> ChangeStatusAsync(Guid id, ReviewStatus status, CancellationToken ct = default);
    Task DeleteReviewAsync(Guid reviewId, Func<Task>? onSuccess = null);
}