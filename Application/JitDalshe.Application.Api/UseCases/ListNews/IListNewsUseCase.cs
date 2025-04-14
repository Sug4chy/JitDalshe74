using CSharpFunctionalExtensions;
using JitDalshe.Application.Api.Dto;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Api.UseCases.ListNews;

public interface IListNewsUseCase
{
    Task<Result<NewsDto[], Error>> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}