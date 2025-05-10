using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
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

    public Task<News[]> ListNewsAsync(CancellationToken ct = default)
    {
        var baseQuery = _dbContext.News
            .Include(x => x.Images)
            .Include(x => x.PrimaryImage)
            .ThenInclude(x => x!.NewsImage)
            .AsQueryable();

        return baseQuery.ToArrayAsync(ct);
    }

    public Task<Maybe<News>> GetNewsByIdAsync(IdOf<News> id, CancellationToken ct = default) 
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
        var oldPrimaryPhoto = await _dbContext.NewsPrimaryPhotos.FirstOrDefaultAsync(x => x.NewsId == news.Id, ct);
        if (oldPrimaryPhoto is not null)
        {
            _dbContext.NewsPrimaryPhotos.Remove(oldPrimaryPhoto);
        }

        _dbContext.News.Update(news);

        if (oldPrimaryPhoto is not null)
        {
            _dbContext.NewsPrimaryPhotos.Add(news.PrimaryImage!);
        }

        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(News news, CancellationToken ct = default)
    {
        if (news.PrimaryImage is not null)
        {
            _dbContext.NewsPrimaryPhotos.Remove(news.PrimaryImage);
        }

        _dbContext.NewsPhotos.RemoveRange(news.Images);
        _dbContext.News.Remove(news);
        await _dbContext.SaveChangesAsync(ct);
    }
}