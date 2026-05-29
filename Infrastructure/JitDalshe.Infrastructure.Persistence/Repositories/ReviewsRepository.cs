using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Repositories;

[Repository]
internal sealed class ReviewsRepository : IReviewsRepository
{
    private readonly PostgresqlDbContext _dbContext;

    public ReviewsRepository(PostgresqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Review[]> FindAllAsync<TOrderKey>(
        int? pageNumber = null, 
        int? pageSize = null, 
        Expression<Func<Review, bool>>? filteringExpression = null,
        Expression<Func<Review, TOrderKey>>? orderByExpression = null, 
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default)
    {
        var query = _dbContext.Reviews.AsQueryable();

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
                _ => throw new ArgumentOutOfRangeException(nameof(sortingOrder), sortingOrder, null)
            };
        }

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return query.ToArrayAsync(ct);
    }

    public Task<Maybe<Review>> FindByIdAsync(IdOf<Review> id, CancellationToken ct = default)
        => _dbContext.Reviews
            .TryFirstAsync(x => x.Id == id, ct);

    public async Task AddAsync(Review review, CancellationToken ct = default)
    {
        _dbContext.Reviews.Add(review);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task EditAsync(Review review, CancellationToken ct = default)
    {
        _dbContext.Reviews.Update(review);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(Review review, CancellationToken ct = default)
    {
        _dbContext.Reviews.Remove(review);
        await _dbContext.SaveChangesAsync(ct);
    }
    
    public async Task<int> CountAsync(Expression<Func<Review, bool>>? filteringExpression = null, CancellationToken ct = default)
    {
        var query = _dbContext.Reviews.AsQueryable();

        if (filteringExpression is not null)
        {
            query = query.Where(filteringExpression);
        }
        
        return await query.CountAsync(ct);
    }
}