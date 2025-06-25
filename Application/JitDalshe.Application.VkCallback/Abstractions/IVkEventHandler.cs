using JitDalshe.Application.VkCallback.Events;

namespace JitDalshe.Application.VkCallback.Abstractions;

public interface IVkEventHandler
{
    string EventType { get; }

    Task HandleAsync(VkEvent @event, CancellationToken ct = default);
}