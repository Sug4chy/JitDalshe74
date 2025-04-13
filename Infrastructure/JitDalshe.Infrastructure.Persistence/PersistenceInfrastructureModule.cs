using Autofac;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Context.Options;

namespace JitDalshe.Infrastructure.Persistence;

public sealed class PersistenceInfrastructureModule : Module
{
    public required string? ConnectionString { get; init; }

    protected override void Load(ContainerBuilder builder)
    {
        LoadDbContext(builder);
    }

    private void LoadDbContext(ContainerBuilder builder)
    {
        builder.Register(_ => new PostgresqlDbContext(
                DbContextOptionsFactory.CreateOptions<PostgresqlDbContext>(ConnectionString)
            ))
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}