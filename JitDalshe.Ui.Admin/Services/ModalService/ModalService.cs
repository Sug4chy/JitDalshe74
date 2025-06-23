using JitDalshe.Ui.Admin.Services.Shared;
using Microsoft.JSInterop;

namespace JitDalshe.Ui.Admin.Services.ModalService;

public sealed class ModalService : IModalService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly Runner _runner;

    public ModalService(IJSRuntime jsRuntime, Runner runner)
    {
        _jsRuntime = jsRuntime;
        _runner = runner;
    }

    public Task ShowModalAsync(string id, CancellationToken ct = default)
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            await _jsRuntime.InvokeVoidAsync(JsInteropConstants.ShowModal, ct, id);
        });

    public Task HideModalAsync(string id, CancellationToken ct = default)
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            await _jsRuntime.InvokeVoidAsync(JsInteropConstants.HideModal, ct, id);
        });
}