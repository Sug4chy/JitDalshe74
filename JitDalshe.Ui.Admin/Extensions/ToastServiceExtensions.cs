using Blazored.Toast.Services;

namespace JitDalshe.Ui.Admin.Extensions;

public static class ToastServiceExtensions
{
    public static void ShowPermanentError(this IToastService toastService, Exception e)
        => toastService.ShowError(e.Message, options =>
        {
            options.ShowProgressBar = false;
            options.DisableTimeout = true;
        });
}