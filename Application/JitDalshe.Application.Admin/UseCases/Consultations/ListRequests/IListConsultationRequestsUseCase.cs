using CSharpFunctionalExtensions;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;

namespace JitDalshe.Application.Admin.UseCases.Consultations.ListRequests;

public interface IListConsultationRequestsUseCase
{
    Task<Result<PagedResult<ConsultationRequestDto>, Error>> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}