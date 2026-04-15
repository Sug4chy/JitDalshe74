using CSharpFunctionalExtensions;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.Consultations.ListRequests;

public interface IListConsultationRequestsUseCase
{
    Task<Result<ConsultationRequestDto[], Error>> ListAsync(CancellationToken ct = default);
}