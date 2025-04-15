using CSharpFunctionalExtensions;
using JitDalshe.Application.AdminApi.Dto;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.AdminApi.UseCases.ListNews;

public interface IListNewsUseCase
{
    Task<Result<NewsDto[], Error>> ListAsync(CancellationToken ct = default);
}