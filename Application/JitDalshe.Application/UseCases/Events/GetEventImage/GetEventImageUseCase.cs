using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.UseCases.Events.GetEventImage;

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

    public async Task<Result<ImageModel, Error>> GetImageAsync(Guid eventId, CancellationToken ct = default)
    {
        try
        {
            var maybeEvent = await _events.FindByIdAsync(IdOf<Event>.From(eventId), ct);
            if (maybeEvent.HasNoValue)
            {
                return Result.Failure<ImageModel, Error>(
                    Error.Of("Событие с таким ID не найдено", ErrorGroup.NotFound));
            }

            var maybeImageStream = await _imageStorage.GetImageByIdAsync(maybeEvent.Value.Image!.Id, ct);
            if (maybeImageStream.HasNoValue)
            {
                return Result.Failure<ImageModel, Error>(
                    Error.Of("Изображения для события не найдено", ErrorGroup.NotFound));
            }

            return Result.Success<ImageModel, Error>(
                new ImageModel(maybeImageStream.Value, maybeEvent.Value.Image.ContentType));
        }
        catch (Exception e)
        {
            return Result.Failure<ImageModel, Error>(Error.Of(e.Message));
        }
    }
}