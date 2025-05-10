using CSharpFunctionalExtensions;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface IEventsRepository
{
    Task<Event[]> FindAllAsync(CancellationToken ct = default);
    Task<Maybe<Event>> FindByIdAsync(IdOf<Event> id, CancellationToken ct = default);
}