using JitDalshe.Ui.Admin.Services.Shared;
using Microsoft.JSInterop;

namespace JitDalshe.Ui.Admin.Services.SwiperService;

public sealed class SwiperService : ISwiperService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly Runner _runner;

    public SwiperService(IJSRuntime jsRuntime, Runner runner)
    {
        _jsRuntime = jsRuntime;
        _runner = runner;
    }

    public Task InitSwiperAsync(CancellationToken ct = default)
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            await _jsRuntime.InvokeVoidAsync(JsInteropConstants.InitSwiper, ct);
        });
}