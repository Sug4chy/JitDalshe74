using System.Net;
using JitDalshe.Ui.Admin.Api.Errors;
using JitDalshe.Ui.Admin.Api.Events;
using JitDalshe.Ui.Admin.Api.Events.Requests;
using JitDalshe.Ui.Admin.Api.News;
using JitDalshe.Ui.Admin.Api.News.Requests;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Models;
using Refit;

namespace JitDalshe.Ui.Admin.Api;

public sealed class ApiFacade : IApiFacade
{
    private readonly INewsApiClient _newsApi;
    private readonly IEventsApiClient _eventsApi;

    public ApiFacade(INewsApiClient newsApi, IEventsApiClient eventsApi)
    {
        _newsApi = newsApi;
        _eventsApi = eventsApi;
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

    public async Task<Event[]> ListEventsAsync(Event[] defaultValue, Action<ApiError> onErrorCallback)
    {
        var response = await _eventsApi.ListEventsAsync();
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

    private static void HandleError(
        HttpStatusCode statusCode,
        ApiException apiError,
        Action<ApiValidationError> onValidationErrorCallback,
        Action<ApiError> onClientErrorCallback,
        Action<ApiError> onServerErrorCallback)
    {
        ApiError error;

        switch ((int)statusCode / 100)
        {
            case 4 when statusCode is HttpStatusCode.BadRequest:
            {
                var validationError = apiError.DeserializeValidationError();
                onValidationErrorCallback(validationError);
                return;
            }
            case 4:
                error = apiError.DeserializeError();
                onClientErrorCallback(error);
                return;
            case 5:
                error = apiError.DeserializeError();
                onServerErrorCallback(error);
                return;
        }
    }

    public async Task<bool> CreateEventAsync(
        CreateEventRequest request,
        Action<ApiValidationError> onValidationErrorCallback,
        Action<ApiError> onClientErrorCallback,
        Action<ApiError> onServerErrorCallback)
    {
        var response = await _eventsApi.CreateEventAsync(request);

        switch (response.StatusCode)
        {
            case HttpStatusCode.Created:
                return true;
            default:
                HandleError(
                    response.StatusCode,
                    response.Error!,
                    onValidationErrorCallback,
                    onClientErrorCallback,
                    onServerErrorCallback
                );
                break;
        }

        return false;
    }

    public async Task<bool> EditEventAsync(
        Guid eventId,
        EditEventRequest request,
        Action<ApiValidationError> onValidationErrorCallback,
        Action<ApiError> onClientErrorCallback,
        Action<ApiError> onServerErrorCallback)
    {
        var response = await _eventsApi.EditEventAsync(eventId, request);

        switch (response.StatusCode)
        {
            case HttpStatusCode.Created:
                return true;
            default:
                HandleError(
                    response.StatusCode,
                    response.Error!,
                    onValidationErrorCallback,
                    onClientErrorCallback,
                    onServerErrorCallback
                );
                break;
        }

        return false;
    }

    public async Task<bool> ReplaceEventImageAsync(
        Guid eventId,
        ReplaceEventImageRequest request,
        Action<ApiValidationError> onValidationErrorCallback,
        Action<ApiError> onClientErrorCallback,
        Action<ApiError> onServerErrorCallback)
    {
        var response = await _eventsApi.ReplaceEventImageAsync(eventId, request);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                return true;
            default:
                HandleError(
                    response.StatusCode,
                    response.Error!,
                    onValidationErrorCallback,
                    onClientErrorCallback,
                    onServerErrorCallback
                );
                break;
        }

        return false;
    }

    public async Task<bool> DeleteEventAsync(
        Guid eventId, 
        Action<ApiError> onClientErrorCallback,
        Action<ApiError> onServerErrorCallback)
    {
        var response = await _eventsApi.DeleteEventAsync(eventId);

        switch (response.StatusCode)
        {
            case HttpStatusCode.NoContent:
                return true;
            default:
                HandleError(
                    response.StatusCode,
                    response.Error!,
                    null!,
                    onClientErrorCallback,
                    onServerErrorCallback
                );
                break;
        }

        return false;
    }
}