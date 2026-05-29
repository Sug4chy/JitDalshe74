using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Domain.Entities.Reviews;

namespace JitDalshe.Application.Site.UseCases.Reviews.LeaveReview;

[UseCase]
internal sealed class LeaveReviewUseCase : ILeaveReviewUseCase
{
    private readonly IReviewsRepository _reviews;

    public LeaveReviewUseCase(IReviewsRepository reviews)
    {
        _reviews = reviews;
    }

    public async Task<LeaveReviewResult> LeaveAsync(
        string reviewerName, 
        int reviewerAge, 
        ReviewerStatus reviewerStatus, 
        string text,
        CancellationToken ct = default)
    {
        try
        {
            await _reviews.AddAsync(
                Review.Create(reviewerName, reviewerAge, reviewerStatus, text), 
                ct);

            return LeaveReviewResult.Success();
        }
        catch (Exception e)
        {
            return  LeaveReviewResult.Failed(e.Message);
        }
    }
}