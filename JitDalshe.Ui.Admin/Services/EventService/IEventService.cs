using JitDalshe.Ui.Admin.Api.Events.Requests;
using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Services.EventService;

public interface IEventService
{
    Task<Event[]> FindAllAsync();
    Task<bool> CreateEventAsync(CreateEventRequest request);
    Task<bool> EditEventAsync(Guid id, EditEventRequest request);
    Task<bool> ReplaceEventImageAsync(Guid eventId, ReplaceEventImageRequest request);
    Task<bool> DeleteEventAsync(Guid eventId);
}