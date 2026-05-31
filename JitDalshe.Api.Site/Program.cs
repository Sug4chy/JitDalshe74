using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using JitDalshe.Api.Extensions;
using JitDalshe.Application;
using JitDalshe.Application.Site;
using JitDalshe.Infrastructure.Minio;
using JitDalshe.Infrastructure.Notifications;
using JitDalshe.Infrastructure.Persistence;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Minio;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<CommonApplicationModule>();
        containerBuilder.RegisterModule<SiteApplicationModule>();

        containerBuilder.RegisterModule(new PersistenceInfrastructureModule
        {
            ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        });
        containerBuilder.RegisterModule(new MinioInfrastructureModule
        {
            ConfigureClient = configureClient =>
                configureClient
                    .WithEndpoint(builder.Configuration["Minio:Endpoint"])
                    .WithCredentials(builder.Configuration["Minio:AccessKey"], builder.Configuration["Minio:SecretKey"])
                    .WithSSL(false)
                    .Build()
        });
        
        containerBuilder.RegisterModule(new NotificationsInfrastructureModule
        {
            BotToken = builder.Configuration["TelegramBot:Token"] ?? string.Empty,
            WebhookUrl = string.Empty,
            SetWebhook = false, // Отключаем настройку вебхуков бота на стороне сайта
            CertificatePath = string.Empty
        });
    });

builder.Services.AddHealthChecks();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddSwaggerGenWithControllerGroups<Program>();
builder.Services.AddControllers();

builder.Services.AddCors();

builder.Services.AddExceptionHandling();

var app = builder.Build();

app.UseExceptionHandling();

app.MapHealthChecks("/health");

app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PostgresqlDbContext>();
    
    if (app.Environment.IsDevelopment())
    {
        await context.SeedDataAsync(); 
    }
}

app.MapControllers();

app.Run();