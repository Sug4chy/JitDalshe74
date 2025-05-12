using Refit;

namespace JitDalshe.Ui.Admin.Api.Events;

public interface IEventsApiClient
{
    [Get("")]
    Task<IApiResponse<Models.Event[]>> ListEventsAsync();
}