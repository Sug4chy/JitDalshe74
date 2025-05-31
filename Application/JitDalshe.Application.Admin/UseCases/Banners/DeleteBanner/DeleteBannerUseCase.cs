using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Exceptions;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Banners.DeleteBanner;

[UseCase]
internal sealed class DeleteBannerUseCase : IDeleteBannerUseCase
{
    private readonly IBannersRepository _banners;
    private readonly IImageStorage _imageStorage;

    public DeleteBannerUseCase(IBannersRepository banners, IImageStorage imageStorage)
    {
        _banners = banners;
        _imageStorage = imageStorage;
    }

    public async Task<UnitResult<Error>> DeleteAsync(IdOf<Banner> id, CancellationToken ct = default)
    {
        try
        {
            var maybeBanner = await _banners.FindByIdAsync(id, ct);
            if (maybeBanner.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Баннер не найден", ErrorGroup.NotFound));
            }

            await _banners.RemoveBannerAsync(maybeBanner.Value, ct);
            await _imageStorage.RemoveImageAsync(maybeBanner.Value.Image!.Id, ct);

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