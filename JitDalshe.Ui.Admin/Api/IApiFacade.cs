using JitDalshe.Ui.Admin.Api.News.Requests;

namespace JitDalshe.Ui.Admin.Api;

public interface IApiFacade
{
    Task<Models.News[]> ListNewsAsync(Models.News[] defaultValue, Action<ApiError> onErrorCallback);

    Task<Models.News?> EditNewsAsync(
        Guid newsId,
        EditNewsRequest request,
        Models.News? defaultValue,
        Action<ApiValidationError> onValidationErrorCallback,
        Action<ApiError> onNotFoundCallback,
        Action<ApiError> onErrorCallback);

    Task<bool> DeleteNewsByIdAsync(
        Guid newsId,
        Action<ApiError> onNotFoundCallback,
        Action<ApiError> onErrorCallback);
}