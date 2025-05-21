using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.UseCases.Banners.GetBannerImage;

[UseCase]
internal sealed class GetBannerImageUseCase : IGetBannerImageUseCase
{
    private readonly IBannersRepository _banners;
    private readonly IImageStorage _imageStorage;

    public GetBannerImageUseCase(IBannersRepository banners, IImageStorage imageStorage)
    {
        _banners = banners;
        _imageStorage = imageStorage;
    }

    public async Task<Result<ImageModel, Error>> GetAsync(IdOf<Banner> bannerId, CancellationToken ct = default)
    {
        try
        {
            var maybeBanner = await _banners.FindByIdAsync(bannerId, ct);
            if (maybeBanner.HasNoValue)
            {
                return Result.Failure<ImageModel, Error>(Error.Of("Баннер не найден", ErrorGroup.NotFound));
            }

            var maybeImageStream = await _imageStorage.GetImageByIdAsync(maybeBanner.Value.Image!.Id, ct);
            if (maybeImageStream.HasNoValue)
            {
                return Result.Failure<ImageModel, Error>(Error.Of("Изображение не найдено",  ErrorGroup.NotFound));
            }

            return Result.Success<ImageModel, Error>(
                new ImageModel(maybeImageStream.Value, maybeBanner.Value.Image.ContentType));
        }
        catch (Exception e)
        {
            return Result.Failure<ImageModel, Error>(Error.Of(e.Message));
        }
    }
}