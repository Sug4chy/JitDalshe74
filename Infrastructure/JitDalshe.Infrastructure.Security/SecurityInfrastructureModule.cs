using Autofac;
using JitDalshe.Application.Abstractions.Security;
using JitDalshe.Infrastructure.Security.Jwt;

namespace JitDalshe.Infrastructure.Security;

public sealed class SecurityInfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BCryptPasswordHasher>()
            .As<IPasswordHasher>()
            .InstancePerLifetimeScope();

        builder.RegisterType<JwtProvider>()
            .As<IJwtProvider>()
            .InstancePerLifetimeScope();
    }
}