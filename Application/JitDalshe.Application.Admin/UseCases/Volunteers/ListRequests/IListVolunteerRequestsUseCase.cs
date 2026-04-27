using CSharpFunctionalExtensions;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Common;

namespace JitDalshe.Application.Admin.UseCases.Volunteers.ListRequests;

public interface IListVolunteerRequestsUseCase
{
    Task<Result<PagedResult<VolunteerRequestDto>, Error>> ListAsync(
        int pageNumber,
        int pageSize,
        RequestStatus? status = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        CancellationToken ct = default);
}