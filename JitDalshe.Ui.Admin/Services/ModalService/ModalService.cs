using Microsoft.JSInterop;

namespace JitDalshe.Ui.Admin.Services.ModalService;

public sealed class ModalService : IModalService
{
    private readonly IJSRuntime _jsRuntime;

    public ModalService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task ShowModalAsync(string id, CancellationToken ct = default)
    {
        await _jsRuntime.InvokeVoidAsync(JsInteropConstants.ShowModal, ct, id);
    }

    public async Task HideModalAsync(string id, CancellationToken ct = default)
    {
        await _jsRuntime.InvokeVoidAsync(JsInteropConstants.HideModal, ct, id);
    }
}