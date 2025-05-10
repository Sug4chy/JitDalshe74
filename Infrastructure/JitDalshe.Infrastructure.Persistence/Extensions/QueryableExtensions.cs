using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Extensions;

public static class QueryableExtensions
{
    public static async Task<Maybe<TEntity>> TryFirstAsync<TEntity>(
        this IQueryable<TEntity> queryable,
        CancellationToken ct = default)
    {
        var entity = await queryable.FirstOrDefaultAsync(ct);

        return entity is not null ? entity : Maybe<TEntity>.None;
    }

    public static async Task<Maybe<TEntity>> TryFirstAsync<TEntity>(
        this IQueryable<TEntity> queryable,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken ct = default)
    {
        var entity = await queryable.FirstOrDefaultAsync(predicate, ct);

        return entity is not null ? entity : Maybe<TEntity>.None;
    }
}