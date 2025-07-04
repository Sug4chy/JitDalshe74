using System.Linq.Expressions;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Reviews;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface IReviewsRepository
{
    Task<Review[]> FindAllAsync<TOrderKey>(
        int? pageNumber = null,
        int? pageSize = null,
        Expression<Func<Review, bool>>? filteringExpression = null,
        Expression<Func<Review, TOrderKey>>? orderByExpression = null,
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default);

    Task AddAsync(Review review, CancellationToken ct = default);
}