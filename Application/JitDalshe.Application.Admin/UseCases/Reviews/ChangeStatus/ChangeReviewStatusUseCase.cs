using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Reviews.ChangeStatus;

[UseCase]
internal sealed class ChangeReviewStatusUseCase(IReviewsRepository reviews) : IChangeReviewStatusUseCase
{
    public async Task<UnitResult<Error>> ChangeStatusAsync(IdOf<Review> reviewId, ReviewStatus newStatus, CancellationToken ct = default)
    {
        try
        {
            var maybeReview = await reviews.FindByIdAsync(reviewId, ct);
            if (maybeReview.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Отзыв не найден", ErrorGroup.NotFound));
            }

            var review = maybeReview.Value;
            review.ChangeStatus(newStatus);

            await reviews.EditAsync(review, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}