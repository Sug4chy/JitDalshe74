using JitDalshe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Context;

public sealed class PostgresqlDbContext(DbContextOptions<PostgresqlDbContext> options) : DbContext(options)
{
    public DbSet<News> News { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}