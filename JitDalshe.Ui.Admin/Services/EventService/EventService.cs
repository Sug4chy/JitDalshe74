using System.Net;
using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.Errors;
using JitDalshe.Ui.Admin.Api.Events;
using JitDalshe.Ui.Admin.Api.Events.Requests;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Services.Shared;
using Refit;

namespace JitDalshe.Ui.Admin.Services.EventService;

public sealed class EventService : IEventService
{
    private readonly Runner _runner;
    private readonly IEventsApiClient _eventsApi;
    private readonly IToastService _toastService;

    public EventService(Runner runner, IEventsApiClient eventsApi, IToastService toastService)
    {
        _runner = runner;
        _eventsApi = eventsApi;
        _toastService = toastService;
    }

    private void HandleError(
        HttpStatusCode statusCode,
        ApiException apiError)
    {
        ApiError error;

        switch ((int)statusCode / 100)
        {
            case 4 when statusCode is HttpStatusCode.BadRequest:
            {
                var validationError = apiError.DeserializeValidationError();
                _toastService.ShowWarning(validationError.Errors.First().Value.First());
                return;
            }
            case 4:
                error = apiError.DeserializeError();
                _toastService.ShowWarning(error.Message);
                return;
            case 5:
                error = apiError.DeserializeError();
                _toastService.ShowError(error.Message);
                return;
        }
    }

    public Task<Event[]> FindAllAsync()
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            var response = await _eventsApi.ListEventsAsync();

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content!;
                default:
                    var error = response.Error!.DeserializeError();
                    _toastService.ShowError(error.Message);
                    return [];
            }
        }, defaultValue: []);

    public Task<bool> CreateEventAsync(CreateEventRequest request)
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            var response = await _eventsApi.CreateEventAsync(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    return true;
                default:
                    HandleError(
                        statusCode: response.StatusCode,
                        apiError: response.Error!);
                    return false;
            }
        });

    public Task<bool> EditEventAsync(Guid id, EditEventRequest request)
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            var response = await _eventsApi.EditEventAsync(id, request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;
                default:
                    HandleError(
                        statusCode: response.StatusCode,
                        apiError: response.Error!);
                    return false;
            }
        });

    public Task<bool> ReplaceEventImageAsync(Guid eventId, ReplaceEventImageRequest request)
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            var response = await _eventsApi.ReplaceEventImageAsync(eventId, request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;
                default:
                    HandleError(
                        statusCode: response.StatusCode,
                        apiError: response.Error!);
                    return false;
            }
        });

    public Task<bool> DeleteEventAsync(Guid eventId)
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            var response = await _eventsApi.DeleteEventAsync(eventId);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return true;
                default:
                    HandleError(
                        statusCode: response.StatusCode,
                        apiError: response.Error!);
                    return false;
            }
        });
}