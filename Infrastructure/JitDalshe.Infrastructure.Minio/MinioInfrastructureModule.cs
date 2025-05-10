using Autofac;
using JitDalshe.Application.Abstractions.ImageStorage;
using Minio;

namespace JitDalshe.Infrastructure.Minio;

public sealed class MinioInfrastructureModule : Module
{
    public required Action<IMinioClient> ConfigureClient { get; init; }

    protected override void Load(ContainerBuilder builder)
    {
        LoadMinio(builder);
        LoadMinioImageStorage(builder);
    }

    private void LoadMinio(ContainerBuilder builder)
    {
        builder.Register(_ => new MinioClientFactory(ConfigureClient))
            .As<IMinioClientFactory>()
            .SingleInstance();

        builder.Register(context => context.Resolve<IMinioClientFactory>().CreateClient())
            .As<IMinioClient>()
            .SingleInstance();
    }

    private static void LoadMinioImageStorage(ContainerBuilder builder)
    {
        builder.RegisterType<MinioImageStorage>()
            .As<IImageStorage>()
            .InstancePerLifetimeScope();
    }
}