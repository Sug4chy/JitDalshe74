using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Domain.Entities;
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
}