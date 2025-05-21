using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;

namespace JitDalshe.Application.UseCases.GetDisplayingBanners;

public interface IGetDisplayingBannersUseCase
{
    Task<Result<DisplayingBannerModel[], Error>> GetAsync(CancellationToken ct = default);
}