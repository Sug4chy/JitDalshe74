using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Site.Dto.Events;
using JitDalshe.Application.Site.Extensions;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Site.UseCases.Events.GetEvent;

[UseCase]
internal sealed class GetEventUseCase :  IGetEventUseCase
{
    private readonly IEventsRepository _events;

    public GetEventUseCase(IEventsRepository events)
    {
        _events = events;
    }
    
    public async Task<Result<EventDto, Error>> GetAsync(IdOf<Event> id, CancellationToken ct = default)
    {
        try
        {
            var maybeEvent = await _events.FindByIdAsync(id, ct);
            if (maybeEvent.HasNoValue || maybeEvent.Value.Status != EventStatus.Published)
                return Result.Failure<EventDto, Error>(Error.Of("Событие не найдено", ErrorGroup.NotFound));

            return Result.Success<EventDto, Error>(maybeEvent.Value.ToDto());
        }
        catch (Exception e)
        {
            return Result.Failure<EventDto, Error>(Error.Of(e.Message));
        }
    }
}