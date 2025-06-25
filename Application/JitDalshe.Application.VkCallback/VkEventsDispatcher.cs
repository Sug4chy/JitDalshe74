using JitDalshe.Application.VkCallback.Abstractions;
using JitDalshe.Application.VkCallback.Events;
using Microsoft.Extensions.Logging;

namespace JitDalshe.Application.VkCallback;

public sealed class VkEventsDispatcher
{
    private readonly IEnumerable<IVkEventHandler>  _handlers;
    private readonly ILogger<VkEventsDispatcher> _logger;

    public VkEventsDispatcher(IEnumerable<IVkEventHandler> handlers, ILogger<VkEventsDispatcher> logger)
    {
        _handlers = handlers;
        _logger = logger;
    }

    public async Task DispatchAsync(VkEvent @event, CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Got event with ID {EventID} (version {Version})", @event.EventId, @event.V);

            var handler = _handlers.SingleOrDefault(x => x.EventType == @event.Type);
            if (handler is null)
            {
                _logger.LogWarning("Handler of type {EventType} not found", @event.Type);
                return;
            }

            await handler.HandleAsync(@event, ct);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Message}", e.Message);
        }
    }
}