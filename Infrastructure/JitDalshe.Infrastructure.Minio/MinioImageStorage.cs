using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;
using Minio;
using Minio.DataModel.Args;
using Exceptions = Minio.Exceptions;

namespace JitDalshe.Infrastructure.Minio;

public sealed class MinioImageStorage : IImageStorage
{
    private const string EventImagesBucketName = "event-images";

    private readonly IMinioClient _minioClient;

    public MinioImageStorage(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task<Maybe<Stream>> GetImageByIdAsync(IdOf<EventImage> id, CancellationToken ct = default)
    {
        var ms = new MemoryStream();

        try
        {
            bool bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs()
                .WithBucket(EventImagesBucketName), ct);
            if (!bucketExists)
            {
                return Maybe<Stream>.None;
            }

            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(EventImagesBucketName)
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

    public Task RemoveImageAsync(IdOf<EventImage> id, CancellationToken ct = default) 
        => _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(EventImagesBucketName)
            .WithObject(id.ToString()), ct);
}