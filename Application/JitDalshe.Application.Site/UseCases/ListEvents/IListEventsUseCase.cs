using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Site.Dto;

namespace JitDalshe.Application.Site.UseCases.ListEvents;

public interface IListEventsUseCase
{
    Task<Result<EventDto[], Error>> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}