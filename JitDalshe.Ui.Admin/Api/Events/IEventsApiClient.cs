using JitDalshe.Ui.Admin.Api.Events.Requests;
using Refit;

namespace JitDalshe.Ui.Admin.Api.Events;

public interface IEventsApiClient
{
    [Get("")]
    Task<IApiResponse<Models.Event[]>> ListEventsAsync();

    [Post("")]
    Task<IApiResponse> CreateEventAsync([Body] CreateEventRequest request);
}