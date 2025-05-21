using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.Extensions;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Enums;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.Banners.ListBanners;

[UseCase]
internal sealed class ListBannersUseCase : IListBannersUseCase
{
    private readonly IBannersRepository _banners;

    public ListBannersUseCase(IBannersRepository banners)
    {
        _banners = banners;
    }

    public async Task<Result<BannerDto[], Error>> ListAsync(CancellationToken ct = default)
    {
        try
        {
            var banners = await _banners.FindAllAsync(
                orderByExpression: x => x.CreatedAt,
                sortingOrder: SortingOrder.Descending,
                ct: ct);

            return Result.Success<BannerDto[], Error>(
                banners
                    .Select(x => x.ToDto())
                    .ToArray()
            );
        }
        catch (Exception e)
        {
            return Result.Failure<BannerDto[], Error>(Error.Of(e.Message));
        }
    }
}