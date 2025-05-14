using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.News.DeleteNews;

public interface IDeleteNewsUseCase
{
    Task<UnitResult<Error>> DeleteAsync(IdOf<Domain.Entities.News.News> id, CancellationToken ct = default);
}