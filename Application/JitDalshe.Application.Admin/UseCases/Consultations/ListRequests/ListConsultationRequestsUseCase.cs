
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.Extensions;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Enums;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;

namespace JitDalshe.Application.Admin.UseCases.Consultations.ListRequests;

[UseCase]
public sealed class ListConsultationRequestsUseCase : IListConsultationRequestsUseCase
{
    private readonly IConsultationRequestsRepository _requests;

    public ListConsultationRequestsUseCase(IConsultationRequestsRepository requests)
    {
        _requests = requests;
    }

    public async Task<Result<PagedResult<ConsultationRequestDto>, Error>> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        try
        {
            var requests = await _requests.FindAllAsync(
                pageNumber: pageNumber,
                pageSize: pageSize,
                orderByExpression: x => x.CreatedAt,
                sortingOrder: SortingOrder.Descending,
                ct: ct);

            var totalCount = await _requests.CountAsync(null, ct);

            var dtos = requests.Select(x => x.ToDto()).ToArray();
            
            var pagedResult = new PagedResult<ConsultationRequestDto>(
                Items: dtos,
                TotalCount: totalCount,
                PageNumber: pageNumber,
                PageSize: pageSize
            );
            
            return Result.Success<PagedResult<ConsultationRequestDto>, Error>(pagedResult);
        }
        catch (Exception e)
        {
            return Result.Failure<PagedResult<ConsultationRequestDto>, Error>(Error.Of(e.Message));
        }
    }
}