using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Reviews.ModerateReview;

public interface IModerateReviewUseCase
{
    Task<ModerateReviewResult> ModerateAsync(IdOf<Review> reviewId, CancellationToken ct = default);
}