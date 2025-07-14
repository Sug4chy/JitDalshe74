using Autofac;
using Autofac.Extensions.DependencyInjection;
using JitDalshe.Application.TelegramBot;
using JitDalshe.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<TelegramBotApplicationModule>();
        containerBuilder.RegisterModule(new PersistenceInfrastructureModule
        {
            ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        });
    });

builder.Services.AddOpenApi();

var app = builder.Build();

app.Run();