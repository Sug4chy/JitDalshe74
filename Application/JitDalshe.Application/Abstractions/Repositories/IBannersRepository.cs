using JitDalshe.Domain.Entities.Banners;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface IBannersRepository
{
    Task<Banner[]> GetDisplayingBannersAsync(CancellationToken ct = default);
}