using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
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

    public Task<Banner[]> GetDisplayingBannersAsync(CancellationToken ct = default)
        => _dbContext.Banners
            .Where(x => x.DisplayOrder.HasValue)
            .OrderBy(x => x.DisplayOrder!.Value)
            .ToArrayAsync(ct);
}