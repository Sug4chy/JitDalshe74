using CSharpFunctionalExtensions;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.ImageStorage;

public interface IImageStorage
{
    Task<Maybe<Stream>> GetImageByIdAsync<TImage>(IdOf<TImage> id, CancellationToken ct = default)
        where TImage : Entity<IdOf<TImage>>, IImage;

    Task<IdOf<TImage>> SaveImageAsync<TImage>(
        byte[] imageContent,
        string contentType,
        CancellationToken ct = default)
        where TImage : Entity<IdOf<TImage>>, IImage;

    Task RemoveImageAsync(IdOf<EventImage> id, CancellationToken ct = default);
}