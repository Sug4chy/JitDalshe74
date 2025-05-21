using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.UseCases.Banners.GetBannerImage;

public interface IGetBannerImageUseCase
{
    Task<Result<ImageModel, Error>> GetAsync(IdOf<Banner> bannerId, CancellationToken ct = default);
}