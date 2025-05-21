using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.Entities.News;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Context;

public sealed class PostgresqlDbContext(DbContextOptions<PostgresqlDbContext> options) : DbContext(options)
{
    public DbSet<News> News { get; init; }
    public DbSet<NewsImage> NewsPhotos { get; init; }
    public DbSet<NewsPrimaryImage>  NewsPrimaryPhotos { get; init; }
    public DbSet<Event> Events { get; init; }
    public DbSet<EventImage> EventImages { get; init; }
    public DbSet<Banner> Banners { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}