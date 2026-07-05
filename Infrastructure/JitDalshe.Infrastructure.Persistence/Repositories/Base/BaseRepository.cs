using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Repositories.Base;

public abstract class BaseRepository<TEntity>(PostgresqlDbContext dbContext) 
    where TEntity : Entity<IdOf<TEntity>>
{
    protected readonly PostgresqlDbContext DbContext = dbContext;

    public virtual Task<Maybe<TEntity>> FindByIdAsync(IdOf<TEntity> id, CancellationToken ct = default)
        => DbContext.Set<TEntity>().TryFirstAsync(x => x.Id == id, ct);

    public virtual async Task AddAsync(TEntity entity, CancellationToken ct = default)
    {
        DbContext.Set<TEntity>().Add(entity);
        await DbContext.SaveChangesAsync(ct);
    }

    public virtual async Task EditAsync(TEntity entity, CancellationToken ct = default)
    {
        DbContext.Set<TEntity>().Update(entity);
        await DbContext.SaveChangesAsync(ct);
    }

    public virtual async Task RemoveAsync(TEntity entity, CancellationToken ct = default)
    {
        DbContext.Set<TEntity>().Remove(entity);
        await DbContext.SaveChangesAsync(ct);
    }

    public virtual Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? filteringExpression = null,
        CancellationToken ct = default)
        => filteringExpression is not null
            ? DbContext.Set<TEntity>().CountAsync(filteringExpression, ct)
            : DbContext.Set<TEntity>().CountAsync(ct);

    public virtual Task<TEntity[]> FindAllAsync<TOrderKey>(
        int? pageNumber = null,
        int? pageSize = null,
        Expression<Func<TEntity, bool>>? filteringExpression = null,
        Expression<Func<TEntity, TOrderKey>>? orderByExpression = null,
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        if (filteringExpression is not null)
        {
            query = query.Where(filteringExpression);
        }

        if (orderByExpression is not null)
        {
            query = sortingOrder switch
            {
                SortingOrder.Ascending => query.OrderBy(orderByExpression),
                SortingOrder.Descending => query.OrderByDescending(orderByExpression),
                _ => query
            };
        }

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return query.ToArrayAsync(ct);
    }
}