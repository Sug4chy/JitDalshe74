using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Context.Options;

public static class DbContextOptionsFactory
{
    public static DbContextOptions<TDbContext> CreateOptions<TDbContext>(string? connectionString)
        where TDbContext : DbContext
        => new DbContextOptionsBuilder<TDbContext>()
            .UseNpgsql(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
}