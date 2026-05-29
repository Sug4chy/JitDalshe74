using JitDalshe.Application.Abstractions.Notifications;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Site.UseCases.Consultations;

[UseCase]
internal sealed class SignUpForConsultationUseCase : ISignUpForConsultationUseCase
{
    private readonly IConsultationRequestsRepository _consultationRepository;
    private readonly IEmailNotificationsSender _emailSender;

    public SignUpForConsultationUseCase(
        IConsultationRequestsRepository consultationRepository,
        IEmailNotificationsSender emailSender
        )
    {
        _consultationRepository = consultationRepository;
        _emailSender = emailSender;
    }

    public async Task<SignUpForConsultationResult> SignUpAsync(
        string patientName, 
        int patientAge, 
        string? patientPhoneNumber, 
        string? patientEmail,
        CommunicationMethod communicationMethod, 
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
            
            await _consultationRepository.AddAsync(request, ct);
            
            const string emailBody = "На сайте зарегистрирована новая заявка на запись на консультацию. Пожалуйста, проверьте панель администрирования.";
            await _emailSender.SendAsync("Новая заявка на консультацию", emailBody, ct);

            return SignUpForConsultationResult.Success();
        }
        catch (Exception e)
        {
            return SignUpForConsultationResult.Failed(e.Message);
        }
    }
}