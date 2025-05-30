using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.Banners.CreateBanner;

public interface ICreateBannerUseCase
{
    Task<UnitResult<Error>> CreateAsync(
        string title,
        string imageBase64Url,
        bool isClickable = false,
        string? redirectOnClickUrl = null,
        int? displayOrder = null,
        CancellationToken ct = default);
}