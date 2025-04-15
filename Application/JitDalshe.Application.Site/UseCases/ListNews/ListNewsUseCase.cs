using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Site.Extensions;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Site.Dto;

namespace JitDalshe.Application.Site.UseCases.ListNews;

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