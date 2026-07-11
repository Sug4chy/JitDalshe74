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
    private readonly IErrorHandlers _errorHandlers;

    public EventService(
        Runner runner, 
        IEventsApiClient eventsApi, 
        IToastService toastService, 
        IErrorHandlers errorHandlers)
    {
        _runner = runner;
        _eventsApi = eventsApi;
        _errorHandlers = errorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }
    
    public Task<PagedResult<Event>?> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _eventsApi.ListEventsAsync(pageNumber, pageSize, ct);
            return response.Handle(_errorHandlers);
        }, defaultValue: null);

    public Task CreateEventAsync(CreateEventRequest request, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _eventsApi.CreateEventAsync(request);
            if (response.Handle(_errorHandlers, HttpStatusCode.Created))
            {
                await (onSuccess?.Invoke() ?? Task.CompletedTask);
            }
        });

    public Task EditEventAsync(Guid id, EditEventRequest request, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _eventsApi.EditEventAsync(id, request);
            if (response.Handle(_errorHandlers))
            {
                await (onSuccess?.Invoke() ?? Task.CompletedTask);
            }
        });

    public Task ReplaceEventImageAsync(Guid eventId, ReplaceEventImageRequest request, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _eventsApi.ReplaceEventImageAsync(eventId, request);
            if (response.Handle(_errorHandlers))
            {
                await (onSuccess?.Invoke() ?? Task.CompletedTask);
            }
        });

    public Task DeleteEventAsync(Guid eventId, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _eventsApi.DeleteEventAsync(eventId);
            if (response.Handle(_errorHandlers, HttpStatusCode.NoContent))
            {
                await (onSuccess?.Invoke() ?? Task.CompletedTask);
            }
        });
}