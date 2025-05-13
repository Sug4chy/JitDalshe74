using CSharpFunctionalExtensions;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.ImageStorage;

public interface IImageStorage
{
    Task<Maybe<Stream>> GetImageByIdAsync(IdOf<EventImage> id, CancellationToken ct = default);

    Task<IdOf<EventImage>> SaveImage(
        byte[] imageContent,
        string contentType,
        CancellationToken ct = default);
}