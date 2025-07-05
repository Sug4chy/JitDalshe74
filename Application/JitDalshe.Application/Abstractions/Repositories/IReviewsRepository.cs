using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.ValueObjects;

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

    Task<Maybe<Review>> FindByIdAsync(IdOf<Review> id, CancellationToken ct = default);
    Task AddAsync(Review review, CancellationToken ct = default);
    Task EditAsync(Review review, CancellationToken ct = default);
}