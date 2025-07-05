using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Reviews.DeleteReview;

public interface IDeleteReviewUseCase
{
    Task<DeleteReviewResult> DeleteAsync(IdOf<Review> reviewId, CancellationToken ct = default);
}