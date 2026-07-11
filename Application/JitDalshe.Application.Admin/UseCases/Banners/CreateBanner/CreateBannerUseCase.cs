using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Banners.CreateBanner;

[UseCase]
internal sealed class CreateBannerUseCase : ICreateBannerUseCase
{
    private readonly IBannersRepository _banners;
    private readonly IImageStorage _imageStorage;
    private readonly string _imageUrlTemplate;

    public CreateBannerUseCase(
        IBannersRepository banners,
        IImageStorage imageStorage,
        string imageUrlTemplate)
    {
        _banners = banners;
        _imageStorage = imageStorage;
        _imageUrlTemplate = imageUrlTemplate;
    }

    public async Task<UnitResult<Error>> CreateAsync(
        string? title,
        string? description,
        string imageBase64Url,
        string mobileImageBase64Url,
        bool isClickable = false,
        string? redirectOnClickUrl = null,
        int? displayOrder = null,
        CancellationToken ct = default)
    {
        try
        {
            if (isClickable && !IsSafeUrl(redirectOnClickUrl))
            {
                return UnitResult.Failure(Error.Of("Недопустимый формат ссылки."));
            }
            
            if (displayOrder is not null)
            {
                var displayingBanners = await _banners.FindDisplayingBannersAsync(ct);
                if (displayingBanners.Any(x => x.DisplayOrder == displayOrder))
                {
                    var bannerToChange = displayingBanners.First(x => x.DisplayOrder == displayOrder);
                    bannerToChange.DisplayOrder = null;
                    await _banners.EditBannerAsync(bannerToChange, ct);
                }
            }

            string imageContentType = imageBase64Url[(imageBase64Url.IndexOf(':') + 1)..imageBase64Url.IndexOf(';')];
            string imageContentString = imageBase64Url.Split(',')[1];
            byte[] imageBytes = Convert.FromBase64String(imageContentString);
            var imageId = await _imageStorage.SaveImageAsync<BannerImage>(imageBytes, imageContentType, ct);
            
            string mobileImageContentType = mobileImageBase64Url[(mobileImageBase64Url.IndexOf(':') + 1)..mobileImageBase64Url.IndexOf(';')];
            string mobileImageContentString = mobileImageBase64Url.Split(',')[1];
            byte[] mobileImageBytes = Convert.FromBase64String(mobileImageContentString);
            var mobileImageId = await _imageStorage.SaveImageAsync<BannerMobileImage>(mobileImageBytes, mobileImageContentType, ct);
            
            var bannerId = IdOf<Banner>.New();

            var image = BannerImage.Create(
                id: imageId,
                url: _imageUrlTemplate.Replace("[id]", bannerId.ToString()).Replace("[entity]", "banners"),
                contentType: imageContentType,
                bannerId: bannerId);
            var mobileImage = BannerMobileImage.Create(
                id: mobileImageId,
                url: _imageUrlTemplate.Replace("[id]", bannerId.ToString()).Replace("[entity]", "banners") + "?isMobile=true",
                contentType: mobileImageContentType,
                bannerId: bannerId);
            
            var banner = Banner.Create(
                id: bannerId,
                title: title,
                description: description,
                redirectOnClickUrl: redirectOnClickUrl,
                displayOrder: displayOrder,
                image: image,
                mobileImage: mobileImage,
                status: displayOrder is null ? BannerStatus.NotPublished : BannerStatus.Published);

            await _banners.AddAsync(banner, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            return UnitResult.Failure(Error.Of(ex.Message));
        }
    }
    
    private static bool IsSafeUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;

        return url.StartsWith("/") || 
               url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || 
               url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
    }
}