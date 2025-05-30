using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Banners.CreateBanner;

[UseCase]
public sealed class CreateBannerUseCase : ICreateBannerUseCase
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
        string title,
        string imageBase64Url,
        bool isClickable = false,
        string? redirectOnClickUrl = null,
        int? displayOrder = null,
        CancellationToken ct = default)
    {
        try
        {
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
            var bannerId = IdOf<Banner>.New();

            var image = new BannerImage(imageId)
            {
                Url = _imageUrlTemplate.Replace("[id]", bannerId.ToString()).Replace("[entity]", "banners"),
                BannerId = bannerId,
                ContentType = imageContentType
            };
            var banner = new Banner(bannerId)
            {
                Title = title,
                IsClickable = isClickable,
                RedirectOnClickUrl = redirectOnClickUrl,
                DisplayOrder = displayOrder,
                Image = image
            };

            await _banners.AddAsync(banner, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            return UnitResult.Failure(Error.Of(ex.Message));
        }
    }
}