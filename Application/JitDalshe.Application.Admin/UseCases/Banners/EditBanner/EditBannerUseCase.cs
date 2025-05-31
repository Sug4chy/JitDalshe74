using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Banners.EditBanner;

[UseCase]
internal sealed class EditBannerUseCase : IEditBannerUseCase
{
    private readonly IBannersRepository _banners;

    public EditBannerUseCase(IBannersRepository banners)
    {
        _banners = banners;
    }

    public async Task<UnitResult<Error>> EditAsync(
        IdOf<Banner> bannerId,
        string title,
        bool isClickable = false,
        string? redirectOnClickUrl = null,
        int? displayOrder = null,
        CancellationToken ct = default)
    {
        try
        {
            var maybeBanner = await _banners.FindByIdAsync(bannerId, ct);
            if (maybeBanner.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Баннер не найден", ErrorGroup.NotFound));
            }

            var banner = maybeBanner.Value;
            banner.Title = title;
            banner.IsClickable = isClickable;
            banner.RedirectOnClickUrl = redirectOnClickUrl;
            banner.DisplayOrder = displayOrder;

            await _banners.EditBannerAsync(banner, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}