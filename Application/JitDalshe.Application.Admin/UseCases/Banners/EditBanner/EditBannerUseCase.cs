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
        string? title,
        string? description,
        BannerStatus status,
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

            if (isClickable && !IsSafeUrl(redirectOnClickUrl))
            {
                return UnitResult.Failure(Error.Of("Недопустимый формат ссылки. Ссылка должна начинаться с http://, https:// или /"));
            }
            
            var banner = maybeBanner.Value;
            banner.Title = title;
            banner.Description = description;
            banner.IsClickable = isClickable;
            banner.RedirectOnClickUrl = redirectOnClickUrl;
            banner.DisplayOrder = displayOrder;
            banner.Status = status;

            await _banners.EditBannerAsync(banner, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
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