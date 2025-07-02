using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Services.Shared;
using Microsoft.JSInterop;

namespace JitDalshe.Ui.Admin.Services.SwiperService;

public sealed class SwiperService : ISwiperService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly Runner _runner;

    public SwiperService(IJSRuntime jsRuntime, Runner runner, IToastService toastService)
    {
        _jsRuntime = jsRuntime;
        _runner = runner;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }

    public Task InitSwiperAsync(CancellationToken ct = default)
        => _runner.RunCatchingAsync(async () =>
        {
            await _jsRuntime.InvokeVoidAsync(JsInteropConstants.InitSwiper, ct);
        });
}