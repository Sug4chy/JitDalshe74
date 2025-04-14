using Autofac;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Context.Options;

namespace JitDalshe.Infrastructure.Persistence;

public sealed class PersistenceInfrastructureModule : Module
{
    public required string? ConnectionString { get; init; }

    protected override void Load(ContainerBuilder builder)
    {
        LoadDbContext(builder);
        LoadRepositories(builder);
    }

    private void LoadDbContext(ContainerBuilder builder)
    {
        builder.Register(_ => new PostgresqlDbContext(
                DbContextOptionsFactory.CreateOptions<PostgresqlDbContext>(ConnectionString)
            ))
            .AsSelf()
            .InstancePerLifetimeScope();
    }

    private void LoadRepositories(ContainerBuilder builder)
    {
        var repositoriesTypes = GetType().Assembly
            .GetTypes()
            .Where(x => x.GetCustomAttributes(typeof(RepositoryAttribute), false).Length != 0)
            .ToArray();

        builder.RegisterTypes(repositoriesTypes)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}