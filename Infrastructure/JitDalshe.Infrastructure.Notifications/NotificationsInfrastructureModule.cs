using Autofac;
using JitDalshe.Application.Abstractions.Notifications;
using JitDalshe.Infrastructure.Notifications.Initialization;
using JitDalshe.Infrastructure.Notifications.Notifications;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace JitDalshe.Infrastructure.Notifications;

public sealed class NotificationsInfrastructureModule : Module
{
    public required string BotToken { get; init; }
    public required string WebhookUrl { get; init; }
    public required bool SetWebhook { get; init; }
    public required string CertificatePath { get; init; }

    protected override void Load(ContainerBuilder builder)
    {
        LoadTelegramBotClient(builder);
        LoadNotificationsSenders(builder);
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

    private static void LoadNotificationsSenders(ContainerBuilder builder)
    {
        builder.RegisterType<TelegramTelegramNotificationsSender>()
            .As<ITelegramNotificationsSender>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<SmtpNotificationSender>()
            .As<IEmailNotificationsSender>()
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