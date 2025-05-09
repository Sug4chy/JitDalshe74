using CSharpFunctionalExtensions;
using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface INewsRepository
{
    Task<News[]> ListNewsAsync(
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken ct = default);

    Task<Maybe<News>> GetNewsByIdAsync(IdOf<News> id, CancellationToken ct = default);
    Task AddAsync(News news, CancellationToken ct = default);
    Task EditAsync(News news, CancellationToken ct = default);
    Task DeleteAsync(News news, CancellationToken ct = default);
}