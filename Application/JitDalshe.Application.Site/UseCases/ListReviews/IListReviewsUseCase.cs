namespace JitDalshe.Application.Site.UseCases.ListReviews;

public interface IListReviewsUseCase
{
    Task<ListReviewsResult> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}