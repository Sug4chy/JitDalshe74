using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Volunteers;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Site.UseCases.Volunteers;

[UseCase]
internal sealed class SignUpForVolunteerUseCase : ISignUpForVolunteerUseCase
{
    private readonly IVolunteerRequestsRepository _repository;

    public SignUpForVolunteerUseCase(
        IVolunteerRequestsRepository repository
        )
    {
        _repository = repository;
    }

    public async Task<SignUpForVolunteerResult> SignUpAsync(
        string applicantName,
        int applicantAge,
        string? applicantPhoneNumber,
        string? applicantEmail,
        CommunicationMethod communicationMethods,
        CancellationToken ct = default)
    {
        try
        {
            var request = VolunteerRequest.Create(
                id: IdOf<VolunteerRequest>.New(),
                applicantName: applicantName,
                applicantAge: applicantAge,
                applicantPhoneNumber: applicantPhoneNumber,
                applicantEmail: applicantEmail,
                communicationMethods: communicationMethods);
            
            await _repository.AddAsync(request, ct);
            
            return SignUpForVolunteerResult.Success();
        }
        catch (Exception e)
        {
            return SignUpForVolunteerResult.Failed(e.Message);
        }
    }
}