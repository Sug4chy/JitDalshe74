using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Extensions;
using Microsoft.JSInterop;

namespace JitDalshe.Ui.Admin.Services.ModalService;

public sealed class ModalService : IModalService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IToastService _toastService;

    public ModalService(IJSRuntime jsRuntime, IToastService toastService)
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

    public Task ShowModalAsync(string id, CancellationToken ct = default)
        => RunShowingToastOnExceptionAsync(async () =>
        {
            await _jsRuntime.InvokeVoidAsync(JsInteropConstants.ShowModal, ct, id);
        });

    public Task HideModalAsync(string id, CancellationToken ct = default)
        => RunShowingToastOnExceptionAsync(async () =>
        {
            await _jsRuntime.InvokeVoidAsync(JsInteropConstants.HideModal, ct, id);
        });
}