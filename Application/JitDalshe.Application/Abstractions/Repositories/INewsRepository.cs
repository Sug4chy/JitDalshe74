using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface INewsRepository
{
    Task<News[]> FindAllAsync<TOrderKey>(
        int? pageNumber = null,
        int? pageSize = null,
        Expression<Func<News, bool>>? filteringExpression = null,
        Expression<Func<News, TOrderKey>>? orderByExpression = null,
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default);

    Task<Maybe<News>> FindByIdAsync(IdOf<News> id, CancellationToken ct = default);
    Task AddAsync(News news, CancellationToken ct = default);
    Task EditAsync(News news, CancellationToken ct = default);
    Task DeleteAsync(News news, CancellationToken ct = default);
}