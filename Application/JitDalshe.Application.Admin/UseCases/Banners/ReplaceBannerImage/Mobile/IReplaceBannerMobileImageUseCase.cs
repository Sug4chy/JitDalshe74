using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Banners.ReplaceBannerImage.Mobile;

public interface IReplaceBannerMobileImageUseCase
{
    Task<UnitResult<Error>> ReplaceAsync(IdOf<Banner> bannerId, string imageBase64Url, CancellationToken ct = default);
}