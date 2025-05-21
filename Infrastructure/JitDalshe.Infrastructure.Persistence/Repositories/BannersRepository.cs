using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Repositories;

[Repository]
public sealed class BannersRepository : IBannersRepository
{
    private readonly PostgresqlDbContext _dbContext;

    public BannersRepository(PostgresqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Banner[]> FindDisplayingBannersAsync(CancellationToken ct = default)
        => _dbContext.Banners
            .Include(x => x.Image)
            .Where(x => x.DisplayOrder.HasValue)
            .OrderBy(x => x.DisplayOrder!.Value)
            .ToArrayAsync(ct);

    public Task<Maybe<Banner>> FindByIdAsync(IdOf<Banner> id, CancellationToken ct = default)
        => _dbContext.Banners
            .Include(x => x.Image)
            .TryFirstAsync(x => x.Id == id, ct);

    public Task<Banner[]> FindAllAsync<TOrderKey>(
        Expression<Func<Banner, TOrderKey>>? orderByExpression = null,
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default)
    {
        var query = _dbContext.Banners
            .Include(x => x.Image)
            .AsQueryable();

        if (orderByExpression is not null)
        {
            query = sortingOrder switch
            {
                SortingOrder.Ascending => query.OrderBy(orderByExpression),
                SortingOrder.Descending => query.OrderByDescending(orderByExpression),
                _ => throw new ArgumentOutOfRangeException(nameof(sortingOrder), sortingOrder, null)
            };
        }

        return query.ToArrayAsync(ct);
    }
}