using Refit;

namespace JitDalshe.Ui.Admin.Services.ErrorHandlers;

public interface IErrorHandlers
{
    void HandleBadRequest(ApiException exception);
    void HandleNotFound(ApiException exception);
    void HandleInternalServerError(ApiException exception);
}