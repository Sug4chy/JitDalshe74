using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using JitDalshe.Api.Extensions;
using JitDalshe.Application;
using JitDalshe.Application.Admin;
using JitDalshe.Infrastructure.Minio;
using JitDalshe.Infrastructure.Persistence;
using Minio;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<CommonApplicationModule>();
        containerBuilder.RegisterModule(new AdminApplicationModule
        {
            ImageUrlTemplate = $"{builder.Configuration["CurrentURL"]}/[entity]/[id]/image"
        });

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

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddSwaggerGenWithControllerGroups<Program>();
builder.Services.AddControllers();

builder.Services.AddCors();

builder.Services.AddExceptionHandling();

var app = builder.Build();

app.UseExceptionHandling();

app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();