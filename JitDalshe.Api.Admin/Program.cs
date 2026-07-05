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
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using JitDalshe.Infrastructure.Security;

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
        
        containerBuilder.RegisterModule<SecurityInfrastructureModule>();
    });

builder.Services.AddHealthChecks();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddSwaggerGenWithControllerGroups<Program>();
builder.Services.AddControllers();

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is not configured");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCors();

builder.Services.AddExceptionHandling();

var app = builder.Build();

app.UseExceptionHandling();

app.MapHealthChecks("/health").AllowAnonymous();

app.UseRouting();

app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();