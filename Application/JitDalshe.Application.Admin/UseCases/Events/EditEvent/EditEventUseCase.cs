using CSharpFunctionalExtensions;
using Ganss.Xss;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Events.EditEvent;

[UseCase]
internal sealed class EditEventUseCase(IEventsRepository events) : IEditEventUseCase
{
    public async Task<UnitResult<Error>> EditAsync(
        IdOf<Event> id,
        string title,
        string shortDescription,
        string fullText,
        DateOnly? date,
        TimeOnly? time,
        string? location,
        EventStatus status,
        CancellationToken ct = default)
    {
        try
        {
            var maybeEvent = await events.FindByIdAsync(id, ct);
            if (maybeEvent.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Событие не найдено", ErrorGroup.NotFound));
            }

            var @event = maybeEvent.Value;

            var sanitizer = new HtmlSanitizer();
            var sanitizedFullText = sanitizer.Sanitize(fullText);
            
            @event.UpdateEventDetails(title, shortDescription, sanitizedFullText, date, time, location);
            @event.ChangeEventStatus(status);

            await events.EditAsync(@event, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}