using System.Net;
using JitDalshe.Ui.Admin.Api.News;
using JitDalshe.Ui.Admin.Api.News.Requests;
using JitDalshe.Ui.Admin.Extensions;

namespace JitDalshe.Ui.Admin.Api;

public sealed class ApiFacade : IApiFacade
{
    private readonly INewsApiClient _newsApi;

    public ApiFacade(INewsApiClient newsApi)
    {
        _newsApi = newsApi;
    }

    public async Task<Models.News[]> ListNewsAsync(Models.News[] defaultValue, Action<ApiError> onErrorCallback)
    {
        var response = await _newsApi.ListNewsAsync();
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return response.Content!;
            default:
                var error = response.Error!.DeserializeError();
                onErrorCallback(error);
                return defaultValue;
        }
    }

    public async Task<Models.News?> EditNewsAsync(
        Guid newsId,
        EditNewsRequest request,
        Models.News? defaultValue,
        Action<ApiValidationError> onValidationErrorCallback,
        Action<ApiError> onNotFoundCallback,
        Action<ApiError> onErrorCallback)
    {
        var response = await _newsApi.EditNewsAsync(newsId, request);
        ApiError error;

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return response.Content!;
            case HttpStatusCode.BadRequest:
                var validationError = response.Error!.DeserializeValidationError();
                onValidationErrorCallback(validationError);
                return defaultValue;
            case HttpStatusCode.NotFound:
                error = response.Error!.DeserializeError();
                onNotFoundCallback(error);
                return defaultValue;
            default:
                error = response.Error!.DeserializeError();
                onErrorCallback(error);
                return defaultValue;
        }
    }

    public async Task<bool> DeleteNewsByIdAsync(
        Guid newsId,
        Action<ApiError> onNotFoundCallback,
        Action<ApiError> onErrorCallback)
    {
        var response = await _newsApi.DeleteNewsByIdAsync(newsId);
        ApiError error;

        switch (response.StatusCode)
        {
            case HttpStatusCode.NoContent:
                return true;
            case HttpStatusCode.NotFound:
                error = response.Error!.DeserializeError();
                onNotFoundCallback(error);
                return false;
            default:
                error = response.Error!.DeserializeError();
                onErrorCallback(error);
                return false;
        }
    }
}