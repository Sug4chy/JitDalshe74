using Autofac;

namespace JitDalshe.Application.TelegramBot;

public sealed class TelegramBotApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UpdatesHandler>()
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}