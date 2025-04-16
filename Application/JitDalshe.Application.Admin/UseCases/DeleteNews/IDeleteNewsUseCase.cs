using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.DeleteNews;

public interface IDeleteNewsUseCase
{
    Task<UnitResult<Error>> DeleteAsync(IdOf<News> id, CancellationToken ct = default);
}