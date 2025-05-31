using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Banners.EditBanner;

public interface IEditBannerUseCase
{
    Task<UnitResult<Error>> EditAsync(
        IdOf<Banner> bannerId,
        string title,
        bool isClickable = false,
        string? redirectOnClickUrl = null,
        int? displayOrder = null,
        CancellationToken ct = default);
}