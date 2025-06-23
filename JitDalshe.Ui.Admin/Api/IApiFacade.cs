using JitDalshe.Ui.Admin.Api.Errors;
using JitDalshe.Ui.Admin.Api.Events.Requests;
using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Api;

public interface IApiFacade
{
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