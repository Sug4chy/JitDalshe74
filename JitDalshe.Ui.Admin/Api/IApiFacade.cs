using JitDalshe.Ui.Admin.Api.Errors;
using JitDalshe.Ui.Admin.Api.Events.Requests;
using JitDalshe.Ui.Admin.Api.News.Requests;
using JitDalshe.Ui.Admin.Models;

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

    Task<Event[]> ListEventsAsync(Event[] defaultValue, Action<ApiError> onErrorCallback);

    Task<bool> CreateEventAsync(
        CreateEventRequest request,
        Action<ApiValidationError> onValidationErrorCallback,
        Action<ApiError> onClientErrorCallback,
        Action<ApiError> onServerErrorCallback);

    Task<bool> EditEventAsync(
        Guid eventId,
        EditEventRequest request,
        Action<ApiValidationError> onValidationErrorCallback,
        Action<ApiError> onClientErrorCallback,
        Action<ApiError> onServerErrorCallback);

    Task<bool> ReplaceEventImageAsync(
        Guid eventId,
        ReplaceEventImageRequest request,
        Action<ApiValidationError> onValidationErrorCallback,
        Action<ApiError> onClientErrorCallback,
        Action<ApiError> onServerErrorCallback);

    Task<bool> DeleteEventAsync(
        Guid eventId,
        Action<ApiError> onClientErrorCallback,
        Action<ApiError> onServerErrorCallback);
}