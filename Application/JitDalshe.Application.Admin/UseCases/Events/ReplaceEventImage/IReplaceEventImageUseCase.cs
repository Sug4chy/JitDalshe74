using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Events.ReplaceEventImage;

public interface IReplaceEventImageUseCase
{
    Task<UnitResult<Error>> ReplaceAsync(IdOf<Event> eventId, string imageBase64Url, CancellationToken ct = default);
}