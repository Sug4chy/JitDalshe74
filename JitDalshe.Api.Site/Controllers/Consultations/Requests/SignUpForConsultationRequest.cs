using System.Text.RegularExpressions;
using FluentValidation;
using JitDalshe.Domain.Entities.Consultations;

namespace JitDalshe.Api.Site.Controllers.Consultations.Requests;

public readonly record struct SignUpForConsultationRequest(
    string PatientName,
    int PatientAge,
    string? PatientPhoneNumber,
    string? PatientEmail,
    PatientCommunicationMethod CommunicationMethod
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
            .When(x => x.CommunicationMethod.HasFlag(PatientCommunicationMethod.CallAPhone) ||
                       x.CommunicationMethod.HasFlag(PatientCommunicationMethod.WhatsApp) ||
                       x.CommunicationMethod.HasFlag(PatientCommunicationMethod.Telegram))
            .Must(x => PhoneNumberRegex().IsMatch(x!))
            .When(x => x.CommunicationMethod.HasFlag(PatientCommunicationMethod.CallAPhone) ||
                       x.CommunicationMethod.HasFlag(PatientCommunicationMethod.WhatsApp) ||
                       x.CommunicationMethod.HasFlag(PatientCommunicationMethod.Telegram));

        RuleFor(x => x.PatientEmail)
            .NotEmpty()
            .When(x => x.CommunicationMethod.HasFlag(PatientCommunicationMethod.Email))
            .EmailAddress()
            .When(x => x.CommunicationMethod.HasFlag(PatientCommunicationMethod.Email));
    }
}