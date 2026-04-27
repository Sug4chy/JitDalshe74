using System.Text.RegularExpressions;
using FluentValidation;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Consultations;

namespace JitDalshe.Api.Site.Controllers.Consultations.Requests;

public readonly record struct SignUpForConsultationRequest(
    string PatientName,
    int PatientAge,
    string? PatientPhoneNumber,
    string? PatientEmail,
    CommunicationMethod CommunicationMethod
);

public sealed partial class SignUpForConsultationRequestValidator : AbstractValidator<SignUpForConsultationRequest>
{
    [GeneratedRegex("([А-Я]|[а-я]|[A-Z]|[a-z])+")]
    private partial Regex PatientNameRegex();


    [GeneratedRegex(@"\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}")]
    private partial Regex PhoneNumberRegex();

    public SignUpForConsultationRequestValidator()
    {
        RuleFor(x => x.PatientName)
            .NotEmpty()
            .Must(x => x.Split(' ').Length == 2)
            .Must(x => PatientNameRegex().IsMatch(x));

        RuleFor(x => x.PatientAge)
            .GreaterThanOrEqualTo(18)
            .LessThanOrEqualTo(122);

        RuleFor(x => x.PatientPhoneNumber)
            .NotEmpty()
            .When(x => x.CommunicationMethod.HasFlag(CommunicationMethod.CallAPhone) ||
                       x.CommunicationMethod.HasFlag(CommunicationMethod.WhatsApp) ||
                       x.CommunicationMethod.HasFlag(CommunicationMethod.Telegram))
            .Must(x => PhoneNumberRegex().IsMatch(x!))
            .When(x => x.CommunicationMethod.HasFlag(CommunicationMethod.CallAPhone) ||
                       x.CommunicationMethod.HasFlag(CommunicationMethod.WhatsApp) ||
                       x.CommunicationMethod.HasFlag(CommunicationMethod.Telegram));

        RuleFor(x => x.PatientEmail)
            .NotEmpty()
            .When(x => x.CommunicationMethod.HasFlag(CommunicationMethod.Email))
            .EmailAddress()
            .When(x => x.CommunicationMethod.HasFlag(CommunicationMethod.Email));
    }
}