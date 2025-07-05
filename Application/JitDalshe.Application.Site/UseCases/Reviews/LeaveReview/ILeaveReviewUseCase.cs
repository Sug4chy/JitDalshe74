using JitDalshe.Domain.Entities.Reviews;

namespace JitDalshe.Application.Site.UseCases.Reviews.LeaveReview;

public interface ILeaveReviewUseCase
{
    Task<LeaveReviewResult> LeaveAsync(
        string reviewerName,
        int reviewerAge,
        ReviewerStatus reviewerStatus,
        string text,
        CancellationToken ct = default);
}