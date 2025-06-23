using Blazored.Toast;
using JitDalshe.Ui.Admin;
using JitDalshe.Ui.Admin.Api;
using JitDalshe.Ui.Admin.Api.Events;
using JitDalshe.Ui.Admin.Api.News;
using JitDalshe.Ui.Admin.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string? apiBaseUrl = builder.Configuration["Api:BaseUrl"];
if (apiBaseUrl is null)
{
    Console.WriteLine("API base url is empty");
    return;
}

builder.Services.AddScoped<IApiFacade, ApiFacade>();

builder.Services
    .AddRefitClient<INewsApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{apiBaseUrl}/news"));

builder.Services
    .AddRefitClient<IEventsApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{apiBaseUrl}/events"));

builder.Services.AddBlazoredToast();
builder.Services.AddModalService();
builder.Services.AddSwiperService();
builder.Services.AddNewsService();

await builder.Build().RunAsync();