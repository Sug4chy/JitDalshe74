namespace JitDalshe.Application.Site.UseCases.Reviews.ListReviews;

public interface IListReviewsUseCase
{
    Task<ListReviewsResult> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}