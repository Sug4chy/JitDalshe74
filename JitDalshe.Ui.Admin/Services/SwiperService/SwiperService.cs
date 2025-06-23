using Microsoft.JSInterop;

namespace JitDalshe.Ui.Admin.Services.SwiperService;

public sealed class SwiperService : ISwiperService
{
    private readonly IJSRuntime _jsRuntime;

    public SwiperService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitSwiperAsync(CancellationToken ct = default)
    {
        await _jsRuntime.InvokeVoidAsync(JsInteropConstants.InitSwiper, ct);
    }
}