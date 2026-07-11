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
using JitDalshe.Api.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using JitDalshe.Infrastructure.Security;
using JitDalshe.Infrastructure.Security.Jwt;

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
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
                  ?? throw new InvalidOperationException("Конфигурация JWT отсутствует в настройках.");

if (string.IsNullOrEmpty(jwtSettings.Key))
{
    throw new InvalidOperationException("Jwt:Key не настроен.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
        
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("authToken", out var token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var allowedOriginsSection = builder.Configuration.GetSection("Cors:AllowedOrigins");
string[] allowedOrigins = Array.Empty<string>();

if (allowedOriginsSection.Exists())
{
    var originsArray = allowedOriginsSection.Get<string[]>();
    if (originsArray != null && originsArray.Length > 0)
    {
        allowedOrigins = originsArray;
    }
    else
    {
        var originsString = allowedOriginsSection.Get<string>();
        if (!string.IsNullOrWhiteSpace(originsString))
        {
            allowedOrigins = originsString
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToArray();
        }
    }
}

bool isCorsEnabled = allowedOrigins.Length > 0;

if (isCorsEnabled)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("ConfiguredCorsPolicy", policy =>
        {
            policy.WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}

builder.Services.AddExceptionHandling();

var app = builder.Build();

app.UseExceptionHandling();

app.MapHealthChecks("/health").AllowAnonymous();

app.UseRouting();

if (isCorsEnabled)
{
    app.UseCors("ConfiguredCorsPolicy");
}

app.UseAuthentication();
app.UseMiddleware<ActiveAdminUserMiddleware>();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();