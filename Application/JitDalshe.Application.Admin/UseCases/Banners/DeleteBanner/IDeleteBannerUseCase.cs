using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Banners.DeleteBanner;

public interface IDeleteBannerUseCase
{
    Task<UnitResult<Error>> DeleteAsync(IdOf<Banner> id, CancellationToken ct = default);
}