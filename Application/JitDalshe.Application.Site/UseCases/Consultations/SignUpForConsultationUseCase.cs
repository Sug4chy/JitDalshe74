using JitDalshe.Application.Abstractions.Notifications;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Site.UseCases.Consultations;

[UseCase]
internal sealed class SignUpForConsultationUseCase : ISignUpForConsultationUseCase
{
    private readonly IConsultationRequestsRepository _consultationRequests;
    private readonly INotificationsSender _notifications;

    public SignUpForConsultationUseCase(
        IConsultationRequestsRepository consultationRequests, 
        INotificationsSender notifications)
    {
        _consultationRequests = consultationRequests;
        _notifications = notifications;
    }

    public async Task<SignUpForConsultationResult> SignUpAsync(
        string patientName, 
        int patientAge, 
        string? patientPhoneNumber, 
        string? patientEmail,
        PatientCommunicationMethod communicationMethod, 
        CancellationToken ct = default)
    {
        try
        {
            var request = ConsultationRequest.Create(
                id: IdOf<ConsultationRequest>.New(),
                patientName: patientName,
                patientAge: patientAge,
                patientPhoneNumber: patientPhoneNumber,
                patientEmail: patientEmail,
                communicationMethods: communicationMethod);

            await _consultationRequests.AddAsync(request, ct);
            await _notifications.SendAsync("Поступила новая заявка на запись на консультацию", ct);

            return SignUpForConsultationResult.Success();
        }
        catch (Exception e)
        {
            return SignUpForConsultationResult.Failed(e.Message);
        }
    }
}