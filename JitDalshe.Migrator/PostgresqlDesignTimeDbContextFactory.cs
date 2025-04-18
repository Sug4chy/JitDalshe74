using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Context.Options;
using Microsoft.EntityFrameworkCore.Design;

namespace JitDalshe.Migrator;

public sealed class PostgresqlDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PostgresqlDbContext>
{
    public PostgresqlDbContext CreateDbContext(string[] args)
    {
        string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Development;

        var configuration =  new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile($"appsettings.{envName}.json")
            .Build();

        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        var dbContext = new PostgresqlDbContext(
            DbContextOptionsFactory.CreateOptions<PostgresqlDbContext>(connectionString));

        return dbContext;
    }
}