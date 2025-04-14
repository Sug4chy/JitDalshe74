using JitDalshe.Domain.Entities;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface INewsRepository
{
    Task<News[]> ListNewsAsync(
        int? pageNumber = null,
        int? pageSize = null, 
        CancellationToken ct = default);
}