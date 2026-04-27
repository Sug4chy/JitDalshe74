using CSharpFunctionalExtensions;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Consultations;

namespace JitDalshe.Application.Admin.UseCases.Consultations.ListRequests;

public interface IListConsultationRequestsUseCase
{
    Task<Result<PagedResult<ConsultationRequestDto>, Error>> ListAsync(
        int pageNumber, 
        int pageSize, 
        RequestStatus? status = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        CancellationToken ct = default);
}