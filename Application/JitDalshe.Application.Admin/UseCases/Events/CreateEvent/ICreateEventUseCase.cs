using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.Events.CreateEvent;

public interface ICreateEventUseCase
{
    Task<UnitResult<Error>> CreateAsync(
        string title,
        string? description,
        DateTime date,
        string imageBase64Url,
        bool isDisplaying,
        CancellationToken ct = default);
}