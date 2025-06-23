using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Extensions;
using Microsoft.JSInterop;

namespace JitDalshe.Ui.Admin.Services.SwiperService;

public sealed class SwiperService : ISwiperService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IToastService _toastService;

    public SwiperService(IJSRuntime jsRuntime, IToastService toastService)
    {
        _jsRuntime = jsRuntime;
        _toastService = toastService;
    }

    private Task RunShowingToastOnExceptionAsync(Func<Task> action)
    {
        try
        {
            return action();
        }
        catch (Exception e)
        {
            _toastService.ShowPermanentError(e);
            return Task.CompletedTask;
        }
    }

    public Task InitSwiperAsync(CancellationToken ct = default)
        => RunShowingToastOnExceptionAsync(async () =>
        {
            await _jsRuntime.InvokeVoidAsync(JsInteropConstants.InitSwiper, ct);
        });
}