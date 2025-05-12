using CSharpFunctionalExtensions;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.ListEvents;

public interface IListEventsUseCase
{
    Task<Result<EventDto[], Error>> ListAsync(CancellationToken ct = default);
}