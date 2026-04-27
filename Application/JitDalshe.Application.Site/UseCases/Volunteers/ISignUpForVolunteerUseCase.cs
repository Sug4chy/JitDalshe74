using JitDalshe.Domain.Common;

namespace JitDalshe.Application.Site.UseCases.Volunteers;

public interface ISignUpForVolunteerUseCase
{
    Task<SignUpForVolunteerResult> SignUpAsync(
        string applicantName,
        int applicantAge,
        string? applicantPhoneNumber,
        string? applicantEmail,
        CommunicationMethod communicationMethods,
        CancellationToken ct = default);
}