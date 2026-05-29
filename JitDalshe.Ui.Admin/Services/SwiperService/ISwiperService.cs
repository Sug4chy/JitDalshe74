namespace JitDalshe.Ui.Admin.Services.SwiperService;

public interface ISwiperService
{
    Task InitSwiperAsync(string selector, CancellationToken ct = default);
}