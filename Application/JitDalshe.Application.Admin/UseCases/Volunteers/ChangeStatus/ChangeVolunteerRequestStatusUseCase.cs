using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Volunteers;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Volunteers.ChangeStatus;

[UseCase]
public sealed class ChangeVolunteerRequestStatusUseCase : IChangeVolunteerRequestStatusUseCase
{
    private readonly IVolunteerRequestsRepository _volunteerRepository;

    public ChangeVolunteerRequestStatusUseCase(IVolunteerRequestsRepository volunteerRepository)
    {
        _volunteerRepository = volunteerRepository;
    }
    
    public async Task<UnitResult<Error>> EditAsync(IdOf<VolunteerRequest> requestId, RequestStatus newStatus, CancellationToken ct = default)
        
    {
        try
        {
            var maybeRequest = await _volunteerRepository.FindByIdAsync(requestId, ct);
            if (maybeRequest.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Заявка не найдена", ErrorGroup.NotFound));
            }

            var request = maybeRequest.Value;
            
            request.ChangeStatus(newStatus);
            
            await _volunteerRepository.EditAsync(request, ct);
            
            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}