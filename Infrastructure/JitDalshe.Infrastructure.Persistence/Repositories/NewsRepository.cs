using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Domain.Entities;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
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

    public Task<News[]> ListNewsAsync(
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken ct = default)
    {
        var baseQuery = _dbContext.News
            .Include(x => x.Photos)
            .Include(x => x.PrimaryPhoto)
            .ThenInclude(x => x!.NewsPhoto)
            .AsNoTracking();

        if (pageNumber is not null && pageSize is not null)
        {
            baseQuery = baseQuery
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        return baseQuery.ToArrayAsync(ct);
    }

    public async Task<Maybe<News>> GetNewsByIdAsync(IdOf<News> id, CancellationToken ct = default)
    {
        var news = await _dbContext.News
            .Include(x => x.Photos)
            .Include(x => x.PrimaryPhoto)
            .ThenInclude(x => x!.NewsPhoto)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return Maybe<News>.From(news);
    }

    public async Task EditAsync(News news, CancellationToken ct = default)
    {
        _dbContext.News.Update(news);
        await _dbContext.SaveChangesAsync(ct);
    }
}