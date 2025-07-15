using JitDalshe.Domain.Entities.Consultations;

namespace JitDalshe.Application.Site.UseCases.Consultations;

public interface ISignUpForConsultationUseCase
{
    Task<SignUpForConsultationResult> SignUpAsync(
        string patientName,
        int patientAge,
        string? patientPhoneNumber,
        string? patientEmail,
        PatientCommunicationMethod communicationMethod,
        CancellationToken ct = default);
}