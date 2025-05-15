using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Exceptions;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Events.DeleteEvent;

[UseCase]
internal sealed class DeleteEventUseCase : IDeleteEventUseCase
{
    private readonly IEventsRepository _events;
    private readonly IImageStorage _imageStorage;

    public DeleteEventUseCase(IEventsRepository events, IImageStorage imageStorage)
    {
        _events = events;
        _imageStorage = imageStorage;
    }

    public async Task<UnitResult<Error>> DeleteAsync(IdOf<Event> id, CancellationToken ct = default)
    {
        try
        {
            var maybeEvent = await _events.FindByIdAsync(id, ct);
            if (maybeEvent.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Событие не найдено", ErrorGroup.NotFound));
            }

            var @event = maybeEvent.Value;
            await _imageStorage.RemoveImageAsync(@event.Image!.Id, ct);
            await _events.RemoveAsync(@event, ct);

            return UnitResult.Success<Error>();
        }
        catch (ImageNotFoundException)
        {
            return UnitResult.Failure(Error.Of("Изображение не найдено",  ErrorGroup.NotFound));
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}