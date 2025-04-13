using Autofac;
using Autofac.Extensions.DependencyInjection;
using JitDalshe.Application.Api;
using JitDalshe.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<ApiApplicationModule>();
        containerBuilder.RegisterModule(new PersistenceInfrastructureModule
        {
            ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection"),
        });
    });

builder.Services.AddOpenApi();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.MapOpenApi();
}

app.Run();
