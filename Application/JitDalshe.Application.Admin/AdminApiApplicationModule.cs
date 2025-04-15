using Autofac;
using JitDalshe.Application.Attributes;

namespace JitDalshe.Application.Admin;

public sealed class AdminApiApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        LoadUseCases(builder);
    }

    private void LoadUseCases(ContainerBuilder builder)
    {
        var useCasesTypes = GetType().Assembly
            .GetTypes()
            .Where(x => x.GetCustomAttributes(typeof(UseCaseAttribute), false).Length != 0)
            .ToArray();

        builder.RegisterTypes(useCasesTypes)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}