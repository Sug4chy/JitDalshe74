using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Extensions;

namespace JitDalshe.Ui.Admin.Services.Shared;

public sealed class Runner
{
    private readonly IToastService _toastService;

    public Runner(IToastService toastService)
    {
        _toastService = toastService;
    }

    public async Task RunShowingToastOnExceptionAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception e)
        {
            _toastService.ShowPermanentError(e);
        }
    }

    public async Task<T> RunShowingToastOnExceptionAsync<T>(Func<Task<T>> action, T defaultValue = default!)
    {
        try
        {
            return await action();
        }
        catch (Exception e)
        {
            _toastService.ShowPermanentError(e);
            return defaultValue;
        }
    }
}