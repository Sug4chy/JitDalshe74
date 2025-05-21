using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface IBannersRepository
{
    Task<Banner[]> FindDisplayingBannersAsync(CancellationToken ct = default);
    Task<Maybe<Banner>> FindByIdAsync(IdOf<Banner> id,  CancellationToken ct = default);
    Task<Banner[]> FindAllAsync<TOrderKey>(
        Expression<Func<Banner, TOrderKey>>? orderByExpression = null,
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default);
}