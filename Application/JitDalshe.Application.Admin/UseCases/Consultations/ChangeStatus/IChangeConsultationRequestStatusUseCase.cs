using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Consultations.ChangeStatus;

public interface IChangeConsultationRequestStatusUseCase
{
    Task<UnitResult<Error>> EditAsync(IdOf<ConsultationRequest> requestId, CancellationToken ct = default);
}
