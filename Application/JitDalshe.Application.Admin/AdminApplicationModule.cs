using Autofac;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.UseCases.Events.CreateEvent;
using JitDalshe.Application.Attributes;

namespace JitDalshe.Application.Admin;

public sealed class AdminApplicationModule : Module
{
    public required string ImageUrlTemplate { get; init; }

    protected override void Load(ContainerBuilder builder)
    {
        LoadUseCases(builder);
    }

    private void LoadUseCases(ContainerBuilder builder)
    {
        var useCasesTypes = GetType().Assembly
            .GetTypes()
            .Where(x => x.GetCustomAttributes(typeof(UseCaseAttribute), false).Length != 0)
            .Except([typeof(CreateEventUseCase)])
            .ToArray();

        builder.RegisterTypes(useCasesTypes)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        builder.Register(ctx => new CreateEventUseCase(
                imageStorage: ctx.Resolve<IImageStorage>(),
                events: ctx.Resolve<IEventsRepository>(),
                imageUrlTemplate: ImageUrlTemplate))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}