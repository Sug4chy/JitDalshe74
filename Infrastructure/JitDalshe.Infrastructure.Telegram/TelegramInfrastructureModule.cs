using Autofac;
using JitDalshe.Application.Abstractions.Notifications;
using JitDalshe.Infrastructure.Telegram.Initialization;
using JitDalshe.Infrastructure.Telegram.Notifications;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace JitDalshe.Infrastructure.Telegram;

public sealed class TelegramInfrastructureModule : Module
{
    public required string BotToken { get; init; }
    public required string WebhookUrl { get; init; }
    public required bool SetWebhook { get; init; }
    public required string CertificatePath { get; init; }

    protected override void Load(ContainerBuilder builder)
    {
        LoadTelegramBotClient(builder);
        LoadTelegramNotificationsSender(builder);
        if (SetWebhook)
        {
            LoadSetWebhookAsyncInitializer(builder);
        }
    }

    private void LoadTelegramBotClient(ContainerBuilder builder)
    {
        builder.Register(_ => new TelegramBotClient(BotToken))
            .AsImplementedInterfaces()
            .SingleInstance();
    }

    private static void LoadTelegramNotificationsSender(ContainerBuilder builder)
    {
        builder.RegisterType<TelegramNotificationsSender>()
            .As<INotificationsSender>()
            .InstancePerLifetimeScope();
    }

    private void LoadSetWebhookAsyncInitializer(ContainerBuilder builder)
    {
        builder.Register(ctx => new SetupBotAsyncInitializer(
                ctx.Resolve<ITelegramBotClient>(),
                ctx.Resolve<IHostEnvironment>(),
                WebhookUrl,
                CertificatePath))
            .AsImplementedInterfaces()
            .SingleInstance();
    }
}