using CSharpFunctionalExtensions;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface IBannersRepository
{
    Task<Banner[]> FindDisplayingBannersAsync(CancellationToken ct = default);
    Task<Maybe<Banner>> FindByIdAsync(IdOf<Banner> id,  CancellationToken ct = default);
}