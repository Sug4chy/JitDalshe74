using Autofac;
using JitDalshe.Application.VkCallback.Abstractions;

namespace JitDalshe.Application.VkCallback;

public sealed class VkCallbackApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        LoadVkEventHandlers(builder);
    }

    private void LoadVkEventHandlers(ContainerBuilder builder)
    {
        var eventHandlerTypes = GetType().Assembly
            .GetTypes()
            .Where(t => t.IsClosedTypeOf(typeof(IVkEventHandler<>)))
            .ToArray();

        builder.RegisterTypes(eventHandlerTypes)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}