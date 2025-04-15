using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Site.Dto;

namespace JitDalshe.Application.Site.UseCases.ListNews;

public interface IListNewsUseCase
{
    Task<Result<NewsDto[], Error>> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}