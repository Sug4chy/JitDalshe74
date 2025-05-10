using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Site.Models;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Site.UseCases.GetEventImage;

[UseCase]
internal sealed class GetEventImageUseCase : IGetEventImageUseCase
{
    private readonly IEventsRepository _events;
    private readonly IImageStorage _imageStorage;

    public GetEventImageUseCase(IEventsRepository events, IImageStorage imageStorage)
    {
        _events = events;
        _imageStorage = imageStorage;
    }

    public async Task<Result<EventImageModel, Error>> GetImageAsync(Guid eventId, CancellationToken ct = default)
    {
        try
        {
            var maybeEvent = await _events.FindByIdAsync(IdOf<Event>.From(eventId), ct);
            if (maybeEvent.HasNoValue)
            {
                return Result.Failure<EventImageModel, Error>(
                    Error.Of("Событие с таким ID не найдено", ErrorGroup.NotFound));
            }

            var maybeImageStream = await _imageStorage.GetImageByIdAsync(maybeEvent.Value.Image!.Id, ct);
            if (maybeImageStream.HasNoValue)
            {
                return Result.Failure<EventImageModel, Error>(
                    Error.Of("Изображения для события не найдено", ErrorGroup.NotFound));
            }

            return Result.Success<EventImageModel, Error>(
                new EventImageModel(maybeImageStream.Value, maybeEvent.Value.Image.ContentType));
        }
        catch (Exception e)
        {
            return Result.Failure<EventImageModel, Error>(Error.Of(e.Message));
        }
    }
}