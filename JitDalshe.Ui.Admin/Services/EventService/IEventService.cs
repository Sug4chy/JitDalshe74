using JitDalshe.Ui.Admin.Api.Events.Requests;
using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Services.EventService;

public interface IEventService
{
    Task<PagedResult<Event>?> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    Task CreateEventAsync(CreateEventRequest request, Func<Task>? onSuccess = null);
    Task EditEventAsync(Guid id, EditEventRequest request, Func<Task>? onSuccess = null);
    Task ReplaceEventImageAsync(Guid eventId, ReplaceEventImageRequest request, Func<Task>? onSuccess = null);
    Task DeleteEventAsync(Guid eventId, Func<Task>? onSuccess = null);
}