using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Exceptions;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Banners.ReplaceBannerImage.Mobile;

[UseCase]
internal sealed class ReplaceBannerMobileImageUseCase : IReplaceBannerMobileImageUseCase
{
    private readonly IBannersRepository _banners;
    private readonly IImageStorage _imageStorage;

    public ReplaceBannerMobileImageUseCase(IBannersRepository banners, IImageStorage imageStorage)
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
            
            if (banner.MobileImage is not null)
            {
                await _imageStorage.RemoveImageAsync(banner.MobileImage.Id, ct);
            }
            
            var newImageId = await _imageStorage.SaveImageAsync<BannerMobileImage>(imageBytes, imageContentType, ct);
            var newImage = BannerMobileImage.Create(
                id: newImageId,
                url: banner.MobileImage!.Url,
                contentType: imageContentType,
                bannerId: bannerId,
                banner: banner);

            await _banners.ReplaceBannerMobileImageAsync(banner, newImage, ct);

            return UnitResult.Success<Error>();
        }
        catch (ImageNotFoundException)
        {
            return UnitResult.Failure(Error.Of("Изображение не найдено", ErrorGroup.NotFound));
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}