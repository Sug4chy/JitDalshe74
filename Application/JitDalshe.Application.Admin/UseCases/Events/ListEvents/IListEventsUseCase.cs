using CSharpFunctionalExtensions;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;

namespace JitDalshe.Application.Admin.UseCases.Events.ListEvents;

public interface IListEventsUseCase
{
    Task<Result<PagedResult<EventDto>, Error>> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}