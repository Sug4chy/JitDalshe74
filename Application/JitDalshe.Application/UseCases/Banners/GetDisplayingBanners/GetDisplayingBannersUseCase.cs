using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Extensions;
using JitDalshe.Application.Models;

namespace JitDalshe.Application.UseCases.Banners.GetDisplayingBanners;

[UseCase]
internal sealed class GetDisplayingBannersUseCase : IGetDisplayingBannersUseCase
{
    private readonly IBannersRepository _banners;

    public GetDisplayingBannersUseCase(IBannersRepository banners)
    {
        _banners = banners;
    }

    public async Task<Result<DisplayingBannerModel[], Error>> GetAsync(CancellationToken ct = default)
    {
        try
        {
            var banners = await _banners.FindDisplayingBannersAsync(ct);

            return Result.Success<DisplayingBannerModel[], Error>(
                banners
                    .Select(x => x.ToDisplayingModel())
                    .ToArray()
            );
        }
        catch (Exception e)
        {
            return Result.Failure<DisplayingBannerModel[], Error>(Error.Of(e.Message));
        }
    }
}