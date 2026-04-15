
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.Extensions;
using JitDalshe.Application.Enums;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.Consultations.ListRequests;

public class ListConsultationRequestsUseCase : IListConsultationRequestsUseCase
{
    private readonly IConsultationRequestsRepository _requests;

    public ListConsultationRequestsUseCase(IConsultationRequestsRepository requests)
    {
        _requests = requests;
    }

    public async Task<Result<ConsultationRequestDto[], Error>> ListAsync(CancellationToken ct = default)
    {
        try
        {
            var requests = await _requests.FindAllAsync(
                orderByExpression: x => x.CreatedAt,
                sortingOrder: SortingOrder.Descending,
                ct: ct);

            return Result.Success<ConsultationRequestDto[], Error>(
                requests
                    .Select(x => x.ToDto())
                    .ToArray()
            );
        }
        catch (Exception e)
        {
            return Result.Failure<ConsultationRequestDto[], Error>(Error.Of(e.Message));
        }
    }
}