using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Events.DeleteEvent;

public interface IDeleteEventUseCase
{
    Task<UnitResult<Error>> DeleteAsync(IdOf<Event> id, CancellationToken ct = default);
}