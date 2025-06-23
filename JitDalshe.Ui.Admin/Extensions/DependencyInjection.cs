using JitDalshe.Ui.Admin.Services.ModalService;
using JitDalshe.Ui.Admin.Services.NewsService;
using JitDalshe.Ui.Admin.Services.SwiperService;

namespace JitDalshe.Ui.Admin.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddModalService(this IServiceCollection services)
        => services.AddScoped<IModalService, ModalService>();

    public static IServiceCollection AddSwiperService(this IServiceCollection services)
        => services.AddScoped<ISwiperService, SwiperService>();

    public static IServiceCollection AddNewsService(this IServiceCollection services)
        => services.AddScoped<INewsService, NewsService>();
}