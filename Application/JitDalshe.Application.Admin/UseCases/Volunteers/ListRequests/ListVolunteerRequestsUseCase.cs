using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.Extensions;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Enums;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Volunteers;

namespace JitDalshe.Application.Admin.UseCases.Volunteers.ListRequests;

[UseCase]
public sealed class ListVolunteerRequestsUseCase : IListVolunteerRequestsUseCase
{
    private readonly IVolunteerRequestsRepository _requests;

    public ListVolunteerRequestsUseCase(IVolunteerRequestsRepository requests)
    {
        _requests = requests;
    }

    public async Task<Result<PagedResult<VolunteerRequestDto>, Error>> ListAsync(
        int pageNumber, 
        int pageSize, 
        RequestStatus? status = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        CancellationToken ct = default)
    {
        try
        {
            Expression<Func<VolunteerRequest, bool>>? filteringExpression = null;
            
            if (status.HasValue || startDate.HasValue || endDate.HasValue)
            {
                filteringExpression = x =>
                    (!status.HasValue || x.Status == status.Value) &&
                    (!startDate.HasValue || DateOnly.FromDateTime(x.CreatedAt) >= startDate.Value) &&
                    (!endDate.HasValue || DateOnly.FromDateTime(x.CreatedAt.Date) <= endDate.Value);
            }
            
            var requests = await _requests.FindAllAsync(
                pageNumber: pageNumber,
                pageSize: pageSize,
                filteringExpression: filteringExpression,
                orderByExpression: x => x.CreatedAt,
                sortingOrder: SortingOrder.Descending,
                ct: ct);

            var totalCount = await _requests.CountAsync(filteringExpression, ct);

            var dtos = requests.Select(x => x.ToDto()).ToArray();
            
            var pagedResult = new PagedResult<VolunteerRequestDto>(
                Items: dtos,
                TotalCount: totalCount,
                PageNumber: pageNumber,
                PageSize: pageSize
            );
            
            return Result.Success<PagedResult<VolunteerRequestDto>, Error>(pagedResult);
        }
        catch (Exception e)
        {
            return Result.Failure<PagedResult<VolunteerRequestDto>, Error>(Error.Of(e.Message));
        }
    }
}