using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.VkCallback.Abstractions;

public interface IVkEventHandler<in TEvent>
    where TEvent : VkEvent
{
    Task<UnitResult<Error>> HandleAsync(TEvent @event, CancellationToken ct);
}