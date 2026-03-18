using Blazored.Toast;
using JitDalshe.Ui.Admin;
using JitDalshe.Ui.Admin.Api.Banners;
using JitDalshe.Ui.Admin.Api.Events;
using JitDalshe.Ui.Admin.Api.News;
using JitDalshe.Ui.Admin.Api.Reviews;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Services.ErrorHandlers;
using JitDalshe.Ui.Admin.Services.NewsService;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string? apiBaseUrl = builder.Configuration["Api:BaseUrl"];
string? apiAdminBaseUrl = $"{apiBaseUrl}/admin";
if (apiBaseUrl is null)
{
    Console.WriteLine("API base url is empty");
    return;
}

Console.WriteLine($"[DEBUG] Api:BaseUrl = '{apiBaseUrl}'");
Console.WriteLine($"[DEBUG] apiAdminBaseUrl = '{apiAdminBaseUrl}'");

builder.Services
    .AddRefitClient<INewsApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{apiAdminBaseUrl}/news"));

builder.Services
    .AddRefitClient<IEventsApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{apiAdminBaseUrl}/events"));

builder.Services
    .AddRefitClient<IBannersApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{apiAdminBaseUrl}/banners"));

builder.Services
    .AddRefitClient<IReviewsApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{apiAdminBaseUrl}/reviews"));

builder.Services.AddBlazoredToast();
builder.Services.AddRunner();
builder.Services.AddModalService();
builder.Services.AddSwiperService();
builder.Services.AddNewsService();
builder.Services.AddEventService();
builder.Services.AddBannerService();
builder.Services.AddCommonErrorHandlers();
builder.Services.AddReviewService();
builder.Services.AddScoped<NewsService>();
builder.Services.AddScoped<CommonErrorHandlers>();


await builder.Build().RunAsync();