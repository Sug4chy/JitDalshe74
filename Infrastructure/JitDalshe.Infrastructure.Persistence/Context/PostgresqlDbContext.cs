using JitDalshe.Application.Entities;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.Entities.Reviews;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Context;

public sealed class PostgresqlDbContext(DbContextOptions<PostgresqlDbContext> options) : DbContext(options)
{
    public DbSet<News> News { get; init; }
    public DbSet<NewsImage> NewsImages { get; init; }
    public DbSet<NewsPrimaryImage>  NewsPrimaryImages { get; init; }
    public DbSet<Event> Events { get; init; }
    public DbSet<EventImage> EventImages { get; init; }
    public DbSet<Banner> Banners { get; init; }
    public DbSet<BannerImage> BannerImages { get; init; }
    public DbSet<Review> Reviews { get; init; }
    public DbSet<TelegramChat> TelegramChats { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}