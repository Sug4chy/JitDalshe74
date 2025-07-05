using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Reviews.DeleteReview;

[UseCase]
internal sealed class DeleteReviewUseCase : IDeleteReviewUseCase
{
    private readonly IReviewsRepository _reviews;

    public DeleteReviewUseCase(IReviewsRepository reviews)
    {
        _reviews = reviews;
    }

    public async Task<DeleteReviewResult> DeleteAsync(IdOf<Review> reviewId, CancellationToken ct = default)
    {
        try
        {
            var maybeReview = await _reviews.FindByIdAsync(reviewId, ct);
            if (maybeReview.HasNoValue)
            {
                return DeleteReviewResult.ReviewNotFound;
            }

            await _reviews.RemoveAsync(maybeReview.Value, ct);

            return DeleteReviewResult.Success;
        }
        catch (Exception e)
        {
            return DeleteReviewResult.Failure(e.Message);
        }
    }
}