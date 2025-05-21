using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Exceptions;
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

    public Task<Maybe<Stream>> GetImageByIdAsync(IdOf<EventImage> id, CancellationToken ct = default)
        => GetImageByIdAsync(id, EventImagesBucketName, ct);

    public Task<Maybe<Stream>> GetImageByIdAsync(IdOf<BannerImage> id, CancellationToken ct = default)
        => GetImageByIdAsync(id, BannerImagesBucketName, ct);

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

    public async Task<IdOf<EventImage>> SaveImageAsync(
        byte[] imageContent,
        string contentType,
        CancellationToken ct = default)
    {
        bool bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs()
            .WithBucket(EventImagesBucketName), ct);
        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs()
                .WithBucket(EventImagesBucketName), ct);
        }

        var newImageId = IdOf<EventImage>.New();
        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(EventImagesBucketName)
            .WithObject(newImageId.ToString())
            .WithObjectSize(imageContent.LongLength)
            .WithStreamData(new MemoryStream(imageContent))
            .WithContentType(contentType), ct);

        return newImageId;
    }

    public async Task RemoveImageAsync(IdOf<EventImage> id, CancellationToken ct = default)
    {
        try
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(EventImagesBucketName)
                .WithObject(id.ToString()), ct);
        }
        catch (Exceptions.ObjectNotFoundException)
        {
            throw new ImageNotFoundException();
        }
    }
}