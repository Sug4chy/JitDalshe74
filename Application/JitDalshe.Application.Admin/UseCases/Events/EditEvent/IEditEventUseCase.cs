using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Events.EditEvent;

public interface IEditEventUseCase
{
    Task<UnitResult<Error>> EditAsync(
        IdOf<Event> id,
        string title,
        string? description,
        DateTime date,
        bool isDisplaying,
        CancellationToken ct = default);
}