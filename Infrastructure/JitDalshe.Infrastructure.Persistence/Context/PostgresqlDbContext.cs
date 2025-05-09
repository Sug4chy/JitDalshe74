using JitDalshe.Domain.Entities.News;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Context;

public sealed class PostgresqlDbContext(DbContextOptions<PostgresqlDbContext> options) : DbContext(options)
{
    public DbSet<News> News { get; init; }
    public DbSet<NewsPhoto> NewsPhotos { get; init; }
    public DbSet<NewsPrimaryPhoto>  NewsPrimaryPhotos { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}