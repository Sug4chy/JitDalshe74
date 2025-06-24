using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Exceptions;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;
using Minio;
using Minio.DataModel.Args;
using Exceptions = Minio.Exceptions;

namespace JitDalshe.Infrastructure.Minio;

public sealed class MinioImageStorage : IImageStorage
{
    private const string EventImagesBucketName = "event-images";
    private const string BannerImagesBucketName = "banner-images";

    private readonly IMinioClient _minioClient;

    public MinioImageStorage(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public Task<Maybe<Stream>> GetImageByIdAsync<TImage>(IdOf<TImage> id, CancellationToken ct = default)
        where TImage : Entity<IdOf<TImage>>, IImage
        => id switch
        {
            IdOf<EventImage> => GetImageByIdAsync(id, EventImagesBucketName, ct),
            IdOf<BannerImage> => GetImageByIdAsync(id, BannerImagesBucketName, ct),
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };

    private async Task<Maybe<Stream>> GetImageByIdAsync<TImage>(
        IdOf<TImage> id,
        string bucketName,
        CancellationToken ct = default)
        where TImage : Entity<IdOf<TImage>>
    {
        var ms = new MemoryStream();

        try
        {
            bool bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs()
                .WithBucket(bucketName), ct);
            if (!bucketExists)
            {
                return Maybe<Stream>.None;
            }

            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(id.ToString())
                .WithCallbackStream(stream =>
                {
                    stream.CopyTo(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                }), ct);

            return ms;
        }
        catch (Exceptions.ObjectNotFoundException)
        {
            return Maybe<Stream>.None;
        }
    }

    public Task<IdOf<TImage>> SaveImageAsync<TImage>(
        byte[] imageContent,
        string contentType,
        CancellationToken ct = default)
        where TImage : Entity<IdOf<TImage>>, IImage
        => typeof(TImage) switch
        {
            { } eventImageType when eventImageType == typeof(EventImage) =>
                SaveImageAsync<TImage>(imageContent, EventImagesBucketName, contentType, ct),
            { } bannerImageType when bannerImageType == typeof(BannerImage) =>
                SaveImageAsync<TImage>(imageContent, BannerImagesBucketName, contentType, ct),
            _ => throw new ArgumentOutOfRangeException(nameof(TImage), typeof(TImage).Name, null)
        };

    private async Task<IdOf<TImage>> SaveImageAsync<TImage>(
        byte[] imageContent,
        string bucketName,
        string contentType,
        CancellationToken ct = default)
        where TImage : Entity<IdOf<TImage>>, IImage
    {
        bool bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs()
            .WithBucket(bucketName), ct);
        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs()
                .WithBucket(bucketName), ct);
        }

        var newImageId = IdOf<TImage>.New();
        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(newImageId.ToString())
            .WithObjectSize(imageContent.LongLength)
            .WithStreamData(new MemoryStream(imageContent))
            .WithContentType(contentType), ct);

        return newImageId;
    }

    public Task RemoveImageAsync<TImage>(IdOf<TImage> id, CancellationToken ct = default)
        where TImage : Entity<IdOf<TImage>>, IImage
        => id switch
        {
            IdOf<EventImage> => RemoveImageAsync(id, EventImagesBucketName, ct),
            IdOf<BannerImage> => RemoveImageAsync(id, BannerImagesBucketName, ct),
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };

    private async Task RemoveImageAsync<TImage>(IdOf<TImage> id, string bucketName, CancellationToken ct = default)
        where TImage : Entity<IdOf<TImage>>, IImage
    {
        try
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(id.ToString()), ct);
        }
        catch (Exceptions.ObjectNotFoundException)
        {
            throw new ImageNotFoundException();
        }
    }
}