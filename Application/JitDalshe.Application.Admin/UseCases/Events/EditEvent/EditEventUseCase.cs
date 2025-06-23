using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Events.EditEvent;

[UseCase]
internal sealed class EditEventUseCase : IEditEventUseCase
{
    private readonly IEventsRepository _events;

    public EditEventUseCase(IEventsRepository events)
    {
        _events = events;
    }

    public async Task<UnitResult<Error>> EditAsync(
        IdOf<Event> id,
        string title,
        string? description,
        DateTime date,
        bool isDisplaying,
        CancellationToken ct = default)
    {
        try
        {
            var maybeEvent = await _events.FindByIdAsync(id, ct);
            if (maybeEvent.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Событие не найдено", ErrorGroup.NotFound));
            }

            var @event = maybeEvent.Value;

            @event.Title = title;
            @event.Description = description;
            @event.Date = DateOnly.FromDateTime(date);
            @event.IsDisplaying = isDisplaying;

            await _events.EditAsync(@event, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}