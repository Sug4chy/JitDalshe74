using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Reviews.ModerateReview;

[UseCase]
internal sealed class ModerateReviewUseCase : IModerateReviewUseCase
{
    private readonly IReviewsRepository _reviews;

    public ModerateReviewUseCase(IReviewsRepository reviews)
    {
        _reviews = reviews;
    }

    public async Task<ModerateReviewResult> ModerateAsync(IdOf<Review> reviewId, CancellationToken ct = default)
    {
        try
        {
            var maybeReview = await _reviews.FindByIdAsync(reviewId, ct);
            if (maybeReview.HasNoValue)
            {
                return ModerateReviewResult.NotFound();
            }

            var review = maybeReview.Value;
            if (review.IsModerated)
            {
                return ModerateReviewResult.ReviewAlreadyModerated();
            }

            review.IsModerated = true;
            await _reviews.EditAsync(review, ct);

            return ModerateReviewResult.Success();
        }
        catch (Exception e)
        {
            return ModerateReviewResult.Failure(e.Message);
        }
    }
}