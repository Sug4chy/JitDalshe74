using CSharpFunctionalExtensions;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.ImageStorage;

public interface IImageStorage
{
    Task<Maybe<Stream>> GetImageByIdAsync(IdOf<EventImage> id, CancellationToken ct = default);

    Task<IdOf<EventImage>> SaveImageAsync(
        byte[] imageContent,
        string contentType,
        CancellationToken ct = default);

    Task RemoveImageAsync(IdOf<EventImage> id, CancellationToken ct = default);
}