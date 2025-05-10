using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using JitDalshe.Api.Extensions;
using JitDalshe.Application.Site;
using JitDalshe.Infrastructure.Minio;
using JitDalshe.Infrastructure.Persistence;
using Minio;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
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
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddSwaggerGenWithControllerGroups();
builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();