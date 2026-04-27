using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Consultations.ChangeStatus;

public interface IChangeConsultationRequestStatusUseCase
{
    Task<UnitResult<Error>> EditAsync(IdOf<ConsultationRequest> requestId, RequestStatus newStatus, CancellationToken ct = default);
}
