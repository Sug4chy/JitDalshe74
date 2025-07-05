using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.Extensions;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Enums;

namespace JitDalshe.Application.Admin.UseCases.Reviews.ListUnmoderatedReviews;

[UseCase]
internal sealed class ListUnmoderatedReviewsUseCase : IListUnmoderatedReviewsUseCase
{
    private readonly IReviewsRepository _reviews;

    public ListUnmoderatedReviewsUseCase(IReviewsRepository reviews)
    {
        _reviews = reviews;
    }

    public async Task<ListUnmoderatedReviewsResult> ListAsync(CancellationToken ct = default)
    {
        try
        {
            var unmoderatedReviews = await _reviews.FindAllAsync(
                filteringExpression: x => !x.IsModerated,
                orderByExpression: x => x.CreatedAt,
                sortingOrder: SortingOrder.Descending,
                ct: ct);

            return ListUnmoderatedReviewsResult.Success(
                unmoderatedReviews
                    .Select(x => x.ToDto())
                    .ToArray()
            );
        }
        catch (Exception e)
        {
            return ListUnmoderatedReviewsResult.Failure(e.Message);
        }
    }
}