using CSharpFunctionalExtensions;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.News.ListNews;

public interface IListNewsUseCase
{
    Task<Result<NewsDto[], Error>> ListAsync(CancellationToken ct = default);
}