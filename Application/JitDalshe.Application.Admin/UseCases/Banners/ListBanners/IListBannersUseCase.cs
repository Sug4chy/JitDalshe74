using CSharpFunctionalExtensions;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.Banners.ListBanners;

public interface IListBannersUseCase
{
    Task<Result<BannerDto[], Error>> ListAsync(CancellationToken ct = default);
}