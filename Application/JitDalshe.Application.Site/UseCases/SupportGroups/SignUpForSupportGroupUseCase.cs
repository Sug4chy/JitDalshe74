using JitDalshe.Application.Abstractions.Notifications;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.SupportGroups;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Site.UseCases.SupportGroups;

[UseCase]
internal sealed class SignUpForSupportGroupUseCase : ISignUpForSupportGroupUseCase
{
    private readonly ISupportGroupRequestsRepository _supportGroupRepository;
    private readonly IEmailNotificationsSender _emailSender;
    public SignUpForSupportGroupUseCase(
        ISupportGroupRequestsRepository supportGroupRepository, 
        IEmailNotificationsSender emailSender
        )
    {
        _supportGroupRepository = supportGroupRepository;
        _emailSender = emailSender;
    }

    public async Task<SignUpForSupportGroupResult> SignUpAsync(
        string applicantName,
        int applicantAge,
        string? applicantPhoneNumber,
        string? applicantEmail,
        CommunicationMethod communicationMethods,
        CancellationToken ct = default)
    {
        try
        {
            var request = SupportGroupRequest.Create(
                id: IdOf<SupportGroupRequest>.New(),
                applicantName: applicantName,
                applicantAge: applicantAge,
                applicantPhoneNumber: applicantPhoneNumber,
                applicantEmail: applicantEmail,
                communicationMethods: communicationMethods);
            
            await _supportGroupRepository.AddAsync(request, ct);
            
            const string emailBody = "На сайте зарегистрирована новая заявка на запись в группу поддержки. Пожалуйста, проверьте панель администрирования.";
            await _emailSender.SendAsync("Новая заявка в группу поддержки", emailBody, ct);
            
            return SignUpForSupportGroupResult.Success();
        }
        catch (Exception e)
        {
            return SignUpForSupportGroupResult.Failed(e.Message);
        }
    }
}