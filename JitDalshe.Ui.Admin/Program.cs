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


string? apiBaseUrl = builder.Configuration["Api:BaseUrl"]?.TrimEnd('/'); 
var hostUri = new Uri(builder.HostEnvironment.BaseAddress);
var origin = new Uri($"{hostUri.Scheme}://{hostUri.Authority}");

string finalApiUrl = new Uri(origin, apiBaseUrl).ToString();

Console.WriteLine($"[DEBUG] ApiBaseUrl = '{apiBaseUrl}'");
Console.WriteLine($"[DEBUG] Origin = '{origin}'");
Console.WriteLine($"[DEBUG] final:BaseUrl = '{finalApiUrl}'");

void RegisterRefitClient<T>(string path) where T : class =>
    builder.Services.AddRefitClient<T>().ConfigureHttpClient(c => c.BaseAddress = new Uri($"{finalApiUrl}/{path}"));

RegisterRefitClient<INewsApiClient>("news");
RegisterRefitClient<IBannersApiClient>("banners");
RegisterRefitClient<IEventsApiClient>("events");
RegisterRefitClient<IReviewsApiClient>("reviews");

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