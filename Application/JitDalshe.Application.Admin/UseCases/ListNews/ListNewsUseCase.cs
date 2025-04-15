using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.Extensions;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.ListNews;

[UseCase]
internal sealed class ListNewsUseCase : IListNewsUseCase
{
    private readonly INewsRepository _newsRepository;

    public ListNewsUseCase(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<Result<NewsDto[], Error>> ListAsync(CancellationToken ct = default)
    {
        try
        {
            var news = await _newsRepository.ListNewsAsync(ct: ct);

            return Result.Success<NewsDto[], Error>(
                news
                    .Select(x => x.ToDto())
                    .ToArray()
            );
        }
        catch (Exception e)
        {
            return Result.Failure<NewsDto[], Error>(Error.Of(e.Message));
        }
    }
}