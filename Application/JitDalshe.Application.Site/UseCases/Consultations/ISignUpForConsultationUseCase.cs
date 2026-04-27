using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Consultations;

namespace JitDalshe.Application.Site.UseCases.Consultations;

public interface ISignUpForConsultationUseCase
{
    Task<SignUpForConsultationResult> SignUpAsync(
        string patientName,
        int patientAge,
        string? patientPhoneNumber,
        string? patientEmail,
        CommunicationMethod communicationMethod,
        CancellationToken ct = default);
}