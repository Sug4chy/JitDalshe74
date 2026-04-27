using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Volunteers;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Volunteers.ChangeStatus;

public interface IChangeVolunteerRequestStatusUseCase
{
    Task<UnitResult<Error>> EditAsync(IdOf<VolunteerRequest> requestId, RequestStatus newStatus, CancellationToken ct = default);
}