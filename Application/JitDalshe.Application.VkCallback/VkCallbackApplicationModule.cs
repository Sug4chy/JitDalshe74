using Autofac;
using JitDalshe.Application.VkCallback.Abstractions;

namespace JitDalshe.Application.VkCallback;

public sealed class VkCallbackApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        LoadVkEventsDispatcher(builder);
        LoadVkEventHandlers(builder);
    }

    private static void LoadVkEventsDispatcher(ContainerBuilder builder)
    {
        builder.RegisterType<VkEventsDispatcher>()
            .AsSelf()
            .SingleInstance();
    }

    private void LoadVkEventHandlers(ContainerBuilder builder)
    {
        var eventHandlerTypes = GetType().Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo<IVkEventHandler>())
            .Except([typeof(IVkEventHandler)])
            .ToArray();

        builder.RegisterTypes(eventHandlerTypes)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}