using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Extensions;
using Refit;

namespace JitDalshe.Ui.Admin.Services.Shared;

public sealed class CommonErrorHandlers
{
    private readonly IToastService _toastService;

    public CommonErrorHandlers(IToastService toastService)
    {
        _toastService = toastService;
    }

    public void HandleBadRequest(ApiException exception)
    {
        var validationError = exception.DeserializeValidationError();
        _toastService.ShowWarning(validationError.Errors.First().Value.First());
    }

    public void HandleNotFound(ApiException exception)
    {
        var error = exception.DeserializeError();
        _toastService.ShowWarning(error.Message);
    }

    public void HandleInternalServerError(ApiException exception)
    {
        var error = exception.DeserializeError();
        _toastService.ShowError(error.Message);
    }
}