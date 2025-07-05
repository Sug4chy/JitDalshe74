using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Enums;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Site.Extensions;

namespace JitDalshe.Application.Site.UseCases.Reviews.ListReviews;

[UseCase]
internal sealed class ListReviewsUseCase : IListReviewsUseCase
{
    private readonly IReviewsRepository _reviews;

    public ListReviewsUseCase(IReviewsRepository reviews)
    {
        _reviews = reviews;
    }

    public async Task<Reviews.ListReviews.ListReviewsResult> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        try
        {
            var foundReviews = await _reviews.FindAllAsync(
                pageNumber: pageNumber,
                pageSize: pageSize,
                filteringExpression: x => x.IsModerated,
                orderByExpression: x => x.CreatedAt,
                sortingOrder: SortingOrder.Descending,
                ct: ct);

            return Reviews.ListReviews.ListReviewsResult.Success(
                foundReviews
                    .Select(x => x.ToDto())
                    .ToArray()
            );
        }
        catch (Exception e)
        {
            return Reviews.ListReviews.ListReviewsResult.Failure(Error.Of(e.Message));
        }
    }
}