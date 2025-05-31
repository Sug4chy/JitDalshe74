using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Banners.ReplaceBannerImage;

[UseCase]
internal sealed class ReplaceBannerImageUseCase : IReplaceBannerImageUseCase
{
    private readonly IBannersRepository _banners;
    private readonly IImageStorage _imageStorage;

    public ReplaceBannerImageUseCase(IBannersRepository banners, IImageStorage imageStorage)
    {
        _banners = banners;
        _imageStorage = imageStorage;
    }

    public async Task<UnitResult<Error>> ReplaceAsync(
        IdOf<Banner> bannerId, 
        string imageBase64Url, 
        CancellationToken ct = default)
    {
        try
        {
            string imageContentType = imageBase64Url[(imageBase64Url.IndexOf(':') + 1)..imageBase64Url.IndexOf(';')];
            string imageContentString = imageBase64Url.Split(',')[1];
            byte[] imageBytes = Convert.FromBase64String(imageContentString);

            var maybeBanner = await _banners.FindByIdAsync(bannerId, ct);
            if (maybeBanner.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Баннер не найден", ErrorGroup.NotFound));
            }

            var banner = maybeBanner.Value;

            await _imageStorage.RemoveImageAsync(banner.Image!.Id, ct);

            var newImageId = await _imageStorage.SaveImageAsync<BannerImage>(imageBytes, imageContentType, ct);
            var newImage = new BannerImage(newImageId)
            {
                Url = banner.Image.Url,
                ContentType = imageContentType,
                Banner = banner,
                BannerId = bannerId
            };

            await _banners.ReplaceBannerImageAsync(banner, newImage, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}