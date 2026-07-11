using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.Extensions;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Enums;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.News.ListNews;

[UseCase]
internal sealed class ListNewsUseCase(INewsRepository newsRepository) : IListNewsUseCase
{
    public async Task<Result<NewsDto[], Error>> ListAsync(CancellationToken ct = default)
    {
        try
        {
            var news = await newsRepository.FindAllAsync(
                orderByExpression: x => x.PublicationDate,
                sortingOrder: SortingOrder.Descending,
                ct: ct);

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