namespace JitDalshe.Application.Admin.UseCases.Reviews.ListUnmoderatedReviews;

public interface IListUnmoderatedReviewsUseCase
{
    Task<ListUnmoderatedReviewsResult> ListAsync(CancellationToken ct = default);
}