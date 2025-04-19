using JitDalshe.Infrastructure.Persistence.Context.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JitDalshe.Infrastructure.Persistence.Context.Options;

public static class DbContextOptionsFactory
{
    public static DbContextOptions<TDbContext> CreateOptions<TDbContext>(string? connectionString)
        where TDbContext : DbContext
        => new DbContextOptionsBuilder<TDbContext>()
            .UseNpgsql(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .UseLoggerFactory(LoggerFactory.Create(x => x.AddConsole()))
            .AddInterceptors(new UpdateAuditsSaveChangesInterceptor())
            .Options;
}