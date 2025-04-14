using Autofac;
using JitDalshe.Application.Api.Attributes;

namespace JitDalshe.Application.Api;

public sealed class ApiApplicationModule : Module
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