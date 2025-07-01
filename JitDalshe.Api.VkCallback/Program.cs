using Autofac;
using Autofac.Extensions.DependencyInjection;
using JitDalshe.Api.Extensions;
using JitDalshe.Application.VkCallback;
using JitDalshe.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<VkCallbackApplicationModule>();
        containerBuilder.RegisterModule(new PersistenceInfrastructureModule
        {
            ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        });
    });

builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

app.MapControllers();

app.Run();