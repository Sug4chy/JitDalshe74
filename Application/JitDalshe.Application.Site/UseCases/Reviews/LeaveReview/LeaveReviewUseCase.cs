using JitDalshe.Application.Abstractions.Notifications;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Domain.Entities.Reviews;

namespace JitDalshe.Application.Site.UseCases.Reviews.LeaveReview;

[UseCase]
internal sealed class LeaveReviewUseCase : ILeaveReviewUseCase
{
    private readonly IReviewsRepository _reviews;
    private readonly IEmailNotificationsSender _emailSender;
    public LeaveReviewUseCase(
        IReviewsRepository reviews,
        IEmailNotificationsSender emailSender
        )
    {
        _reviews = reviews;
        _emailSender = emailSender;
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

            const string emailBody = "На сайте оставлен новый отзыв. Пожалуйста, проверьте панель администрирования для его модерации.";
            await _emailSender.SendAsync("Новый отзыв на сайте", emailBody, ct);
            
            return LeaveReviewResult.Success();
        }
        catch (Exception e)
        {
            return  LeaveReviewResult.Failed(e.Message);
        }
    }
}