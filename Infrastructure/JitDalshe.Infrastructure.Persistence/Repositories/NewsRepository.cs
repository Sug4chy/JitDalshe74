using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Repositories;

[Repository]
public sealed class NewsRepository : INewsRepository
{
    private readonly PostgresqlDbContext _dbContext;

    public NewsRepository(PostgresqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<News[]> FindAllAsync<TOrderKey>(
        int? pageNumber = null,
        int? pageSize = null,
        Expression<Func<News, bool>>? filteringExpression = null,
        Expression<Func<News, TOrderKey>>? orderByExpression = null,
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default)
    {
        var query = _dbContext.News
            .Include(x => x.Images)
            .Include(x => x.PrimaryImage)
            .ThenInclude(x => x!.NewsImage)
            .AsQueryable();

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

    public Task<Maybe<News>> FindByIdAsync(IdOf<News> id, CancellationToken ct = default)
        => _dbContext.News
            .Include(x => x.Images)
            .Include(x => x.PrimaryImage)
            .ThenInclude(x => x!.NewsImage)
            .TryFirstAsync(x => x.Id == id, ct);

    public async Task AddAsync(News news, CancellationToken ct = default)
    {
        _dbContext.News.Add(news);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task EditAsync(News news, CancellationToken ct = default)
    {
        var oldPrimaryPhoto = await _dbContext.NewsPrimaryImages.FirstOrDefaultAsync(x => x.NewsId == news.Id, ct);
        if (oldPrimaryPhoto is not null)
        {
            _dbContext.NewsPrimaryImages.Remove(oldPrimaryPhoto);
        }

        _dbContext.News.Update(news);

        if (oldPrimaryPhoto is not null)
        {
            _dbContext.NewsPrimaryImages.Add(news.PrimaryImage!);
        }

        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(News news, CancellationToken ct = default)
    {
        if (news.PrimaryImage is not null)
        {
            _dbContext.NewsPrimaryImages.Remove(news.PrimaryImage);
        }

        _dbContext.NewsImages.RemoveRange(news.Images);
        _dbContext.News.Remove(news);
        await _dbContext.SaveChangesAsync(ct);
    }
}