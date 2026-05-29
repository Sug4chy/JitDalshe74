using JitDalshe.Application.Abstractions.Notifications;
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
    private readonly IEmailNotificationsSender _emailSender;
    
    public SignUpForVolunteerUseCase(
        IVolunteerRequestsRepository repository,
        IEmailNotificationsSender emailSender
        )
    {
        _repository = repository;
        _emailSender = emailSender;
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
            
            const string emailBody = "На сайте зарегистрирована новая заявка на волонтерство. Пожалуйста, проверьте панель администрирования.";
            await _emailSender.SendAsync("Новая заявка на волонтерство", emailBody, ct);
            
            return SignUpForVolunteerResult.Success();
        }
        catch (Exception e)
        {
            return SignUpForVolunteerResult.Failed(e.Message);
        }
    }
}