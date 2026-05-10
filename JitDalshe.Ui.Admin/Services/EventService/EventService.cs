using System.Net;
using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.Events;
using JitDalshe.Ui.Admin.Api.Events.Requests;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Services.ErrorHandlers;
using JitDalshe.Ui.Admin.Services.Shared;

namespace JitDalshe.Ui.Admin.Services.EventService;

public sealed class EventService : IEventService
{
    private readonly Runner _runner;
    private readonly IEventsApiClient _eventsApi;
    private readonly CommonErrorHandlers _commonErrorHandlers;

    public EventService(
        Runner runner, 
        IEventsApiClient eventsApi, 
        IToastService toastService, 
        CommonErrorHandlers commonErrorHandlers)
    {
        _runner = runner;
        _eventsApi = eventsApi;
        _commonErrorHandlers = commonErrorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }
    
    public Task<PagedResult<Event>?> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _eventsApi.ListEventsAsync(pageNumber, pageSize, ct);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content;
                case HttpStatusCode.InternalServerError:
                    _commonErrorHandlers.HandleInternalServerError(response.Error!);
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }, defaultValue: null);

    public Task CreateEventAsync(CreateEventRequest request, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _eventsApi.CreateEventAsync(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    await (onSuccess?.Invoke() ?? Task.CompletedTask);
                    break;
                case HttpStatusCode.BadRequest:
                    _commonErrorHandlers.HandleBadRequest(response.Error!);
                    break;
                case HttpStatusCode.InternalServerError:
                    _commonErrorHandlers.HandleInternalServerError(response.Error!);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });

    public Task EditEventAsync(Guid id, EditEventRequest request, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _eventsApi.EditEventAsync(id, request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    await (onSuccess?.Invoke() ?? Task.CompletedTask);
                    break;
                case HttpStatusCode.BadRequest:
                    _commonErrorHandlers.HandleBadRequest(response.Error!);
                    break;
                case HttpStatusCode.NotFound:
                    _commonErrorHandlers.HandleNotFound(response.Error!);
                    break;
                case HttpStatusCode.InternalServerError:
                    _commonErrorHandlers.HandleInternalServerError(response.Error!);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });

    public Task ReplaceEventImageAsync(Guid eventId, ReplaceEventImageRequest request, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _eventsApi.ReplaceEventImageAsync(eventId, request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    await (onSuccess?.Invoke() ?? Task.CompletedTask);
                    break;
                case HttpStatusCode.BadRequest:
                    _commonErrorHandlers.HandleBadRequest(response.Error!);
                    break;
                case HttpStatusCode.NotFound:
                    _commonErrorHandlers.HandleNotFound(response.Error!);
                    break;
                case HttpStatusCode.InternalServerError:
                    _commonErrorHandlers.HandleInternalServerError(response.Error!);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });

    public Task DeleteEventAsync(Guid eventId, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _eventsApi.DeleteEventAsync(eventId);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    await (onSuccess?.Invoke() ?? Task.CompletedTask);
                    break;
                case HttpStatusCode.NotFound:
                    _commonErrorHandlers.HandleNotFound(response.Error!);
                    break;
                case HttpStatusCode.InternalServerError:
                    _commonErrorHandlers.HandleInternalServerError(response.Error!);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
}