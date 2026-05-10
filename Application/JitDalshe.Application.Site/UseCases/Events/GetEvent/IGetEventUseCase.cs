using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Site.Dto.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Site.UseCases.Events.GetEvent;

public interface IGetEventUseCase
{
    Task<Result<EventDto, Error>> GetAsync(IdOf<JitDalshe.Domain.Entities.Events.Event> id, CancellationToken ct = default);
}