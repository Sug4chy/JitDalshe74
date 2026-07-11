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

public sealed class MinioImageStorage(IMinioClient minioClient) : IImageStorage
{
    private const string EventImagesBucketName = "event-images";
    private const string BannerImagesBucketName = "banner-images";
    private const long MaxImageSize = 10 * 1024 * 1024;
    
    public Task<Maybe<Stream>> GetImageByIdAsync<TImage>(IdOf<TImage> id, CancellationToken ct = default)
        where TImage : Entity<IdOf<TImage>>, IImage
        => id switch
        {
            IdOf<EventImage> => GetImageByIdAsync(id, EventImagesBucketName, ct),
            IdOf<BannerImage> => GetImageByIdAsync(id, BannerImagesBucketName, ct),
            IdOf<BannerMobileImage> => GetImageByIdAsync(id, BannerImagesBucketName, ct),
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
            bool bucketExists = await minioClient.BucketExistsAsync(new BucketExistsArgs()
                .WithBucket(bucketName), ct);
            if (!bucketExists)
            {
                return Maybe<Stream>.None;
            }

            await minioClient.GetObjectAsync(new GetObjectArgs()
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
            { } bannerMobileImageType when bannerMobileImageType == typeof(BannerMobileImage) =>
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
        if (imageContent.LongLength > MaxImageSize)
        {
            throw new ArgumentException("Превышен максимальный размер файла (10 МБ)");
        }
        
        if (!IsValidImageSignature(imageContent))
        {
            throw new ArgumentException("Недопустимый формат изображения");
        }
        
        bool bucketExists = await minioClient.BucketExistsAsync(new BucketExistsArgs()
            .WithBucket(bucketName), ct);
        if (!bucketExists)
        {
            await minioClient.MakeBucketAsync(new MakeBucketArgs()
                .WithBucket(bucketName), ct);
        }

        var newImageId = IdOf<TImage>.New();
        await minioClient.PutObjectAsync(new PutObjectArgs()
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
            IdOf<BannerMobileImage> => RemoveImageAsync(id, BannerImagesBucketName, ct),
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };

    private async Task RemoveImageAsync<TImage>(IdOf<TImage> id, string bucketName, CancellationToken ct = default)
        where TImage : Entity<IdOf<TImage>>, IImage
    {
        try
        {
            await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(id.ToString()), ct);
        }
        catch (Exceptions.ObjectNotFoundException)
        {
            throw new ImageNotFoundException();
        }
    }
    
    private static bool IsValidImageSignature(byte[] bytes)
    {
        if (bytes.Length < 4) return false;
        
        if (bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
            return true;
        
        if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
            return true;
        
        if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x38)
            return true;
        
        if (bytes.Length >= 12 &&
            bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46 && 
            bytes[8] == 0x57 && bytes[9] == 0x45 && bytes[10] == 0x42 && bytes[11] == 0x50) 
            return true;

        return false;
    }
}