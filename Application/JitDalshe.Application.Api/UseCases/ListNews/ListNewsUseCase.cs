using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Api.Attributes;
using JitDalshe.Application.Api.Dto;
using JitDalshe.Application.Api.Extensions;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Api.UseCases.ListNews;

[UseCase]
internal sealed class ListNewsUseCase : IListNewsUseCase
{
    private readonly INewsRepository _newsRepository;

    public ListNewsUseCase(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<Result<NewsDto[], Error>> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        try
        {
            var news = await _newsRepository.ListNewsAsync(pageNumber, pageSize, ct);

            return Result.Success<NewsDto[], Error>(news.Select(x => x.ToDto()).ToArray());
        }
        catch (Exception e)
        {
            return Result.Failure<NewsDto[], Error>(Error.Of(e.Message));
        }
    }
}