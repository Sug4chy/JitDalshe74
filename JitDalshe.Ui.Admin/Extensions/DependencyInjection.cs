using System.Net;
using JitDalshe.Ui.Admin.Services.AdminUserService;
using JitDalshe.Ui.Admin.Services.AuthService;
using JitDalshe.Ui.Admin.Services.BannerService;
using JitDalshe.Ui.Admin.Services.ConsultationService;
using JitDalshe.Ui.Admin.Services.ErrorHandlers;
using JitDalshe.Ui.Admin.Services.EventService;
using JitDalshe.Ui.Admin.Services.ModalService;
using JitDalshe.Ui.Admin.Services.NewsService;
using JitDalshe.Ui.Admin.Services.ReviewService;
using JitDalshe.Ui.Admin.Services.Shared;
using JitDalshe.Ui.Admin.Services.SupportGroupService;
using JitDalshe.Ui.Admin.Services.SwiperService;
using JitDalshe.Ui.Admin.Services.VolunteerService;

namespace JitDalshe.Ui.Admin.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddRunner(this IServiceCollection services)
        => services.AddScoped<Runner>();

    public static IServiceCollection AddModalService(this IServiceCollection services)
        => services.AddScoped<IModalService, ModalService>();

    public static IServiceCollection AddSwiperService(this IServiceCollection services)
        => services.AddScoped<ISwiperService, SwiperService>();

    public static IServiceCollection AddNewsService(this IServiceCollection services)
        => services.AddScoped<INewsService, NewsService>();

    public static IServiceCollection AddEventService(this IServiceCollection services)
        => services.AddScoped<IEventService, EventService>();

    public static IServiceCollection AddBannerService(this IServiceCollection services)
        => services.AddScoped<IBannerService, BannerService>();

    public static IServiceCollection AddCommonErrorHandlers(this IServiceCollection services)
        => services.AddScoped<IErrorHandlers, CommonErrorHandlers>();

    public static IServiceCollection AddReviewService(this IServiceCollection services)
        => services.AddScoped<IReviewService, ReviewService>();

    public static IServiceCollection AddConsultationService(this IServiceCollection services)
        => services.AddScoped<IConsultationService, ConsultationService>();
    
    public static IServiceCollection AddSupportGroupService(this IServiceCollection services)
        => services.AddScoped<ISupportGroupService,  SupportGroupService>();
    
    public static IServiceCollection AddVolunteerService(this IServiceCollection services)
        => services.AddScoped<IVolunteerService,  VolunteerService>();
    
    public static IServiceCollection AddAuthService(this IServiceCollection services)
        => services.AddScoped<IAuthService, AuthService>().AddTransient<CookieHandler>();
    
    public static IServiceCollection AddAdminUserService(this IServiceCollection services)
        => services.AddScoped<IAdminUserService, AdminUserService>();
}