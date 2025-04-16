using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.DeleteNews;

[UseCase]
internal sealed class DeleteNewsUseCase : IDeleteNewsUseCase
{
    private readonly INewsRepository _newsRepository;

    public DeleteNewsUseCase(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<UnitResult<Error>> DeleteAsync(IdOf<News> id, CancellationToken ct = default)
    {
        try
        {
            var maybeNews = await _newsRepository.GetNewsByIdAsync(id, ct);
            if (maybeNews.HasNoValue)
            {
                return UnitResult.Failure(Error.Of($"News with id: {id} was not found.", ErrorGroup.NotFound));
            }

            await _newsRepository.DeleteAsync(maybeNews.Value, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}