using JitDalshe.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Tests.IntegrationTests;

public abstract class IntegrationTestBase
{
    protected DbContextOptions<PostgresqlDbContext> CreateNewDatabaseOptions()
    {
        return new DbContextOptionsBuilder<PostgresqlDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }
}