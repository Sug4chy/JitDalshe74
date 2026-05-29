using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Reviews.ChangeStatus;

[UseCase]
internal sealed class ChangeReviewStatusUseCase : IChangeReviewStatusUseCase
{
    private readonly IReviewsRepository _reviews;

    public ChangeReviewStatusUseCase(IReviewsRepository reviews)
    {
        _reviews = reviews;
    }

    public async Task<UnitResult<Error>> ChangeStatusAsync(IdOf<Review> reviewId, ReviewStatus newStatus, CancellationToken ct = default)
    {
        try
        {
            var maybeReview = await _reviews.FindByIdAsync(reviewId, ct);
            if (maybeReview.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Отзыв не найден", ErrorGroup.NotFound));
            }

            var review = maybeReview.Value;
            review.ChangeStatus(newStatus);

            await _reviews.EditAsync(review, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}