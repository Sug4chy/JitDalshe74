using System.Text.RegularExpressions;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Context.Options;
using Microsoft.EntityFrameworkCore;

using var cts = new CancellationTokenSource();
var ct = cts.Token;

string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Development;

var definiteEnvMigrationNameRegex = new Regex(
    $".+__({Environments.Development}|{Environments.Staging}|{Environments.Production}).cs");

var configuration =  new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddJsonFile($"appsettings.{envName}.json")
    .Build();

string? connectionString = configuration.GetConnectionString("DefaultConnection");

await using var dbContext = new PostgresqlDbContext(
    DbContextOptionsFactory.CreateOptions<PostgresqlDbContext>(connectionString));

var pendingMigrationsNames =  await dbContext.Database.GetPendingMigrationsAsync(ct);
foreach (string migrationName in pendingMigrationsNames)
{
    if (definiteEnvMigrationNameRegex.IsMatch(migrationName))
    {
        if (migrationName.EndsWith($"__{envName}.cs"))
        {
            await dbContext.Database.MigrateAsync(migrationName, ct);
        }

        continue;
    }

    await dbContext.Database.MigrateAsync(migrationName, ct);
    Console.WriteLine($"Applied  migration: {migrationName}");
}

Console.WriteLine("Finished");