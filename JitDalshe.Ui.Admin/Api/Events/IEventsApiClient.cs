using JitDalshe.Ui.Admin.Api.Events.Requests;
using JitDalshe.Ui.Admin.Models;
using Refit;

namespace JitDalshe.Ui.Admin.Api.Events;

public interface IEventsApiClient
{
    [Get("")]
    Task<IApiResponse<Event[]>> ListEventsAsync();

    [Post("")]
    Task<IApiResponse> CreateEventAsync([Body] CreateEventRequest request);

    [Patch("/{id}")]
    Task<IApiResponse> EditEventAsync(Guid id, [Body] EditEventRequest request);

    [Patch("/{id}/image")]
    Task<IApiResponse> ReplaceEventImageAsync(Guid id, [Body] ReplaceEventImageRequest request);
}