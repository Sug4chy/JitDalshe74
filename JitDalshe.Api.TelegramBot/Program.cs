using Autofac;
using Autofac.Extensions.DependencyInjection;
using JitDalshe.Application.TelegramBot;
using JitDalshe.Infrastructure.Persistence;
using JitDalshe.Infrastructure.Telegram;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<TelegramBotApplicationModule>();
        containerBuilder.RegisterModule(new PersistenceInfrastructureModule
        {
            ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        });

        containerBuilder.RegisterModule(new TelegramInfrastructureModule
        {
            BotToken = builder.Configuration["TelegramBot:Token"] ?? string.Empty,
            WebhookUrl = builder.Configuration["WEBHOOK_URL"] ?? string.Empty,
            SetWebhook = true,
            CertificatePath = builder.Configuration["WEBHOOK_CERTIFICATE_PATH"] ?? string.Empty
        });
    });

builder.Services.AddOpenApi();

var app = builder.Build();

app.Run();