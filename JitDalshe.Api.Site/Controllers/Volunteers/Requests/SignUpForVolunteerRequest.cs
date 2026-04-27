using System.Text.RegularExpressions;
using FluentValidation;
using JitDalshe.Domain.Common;

namespace JitDalshe.Api.Site.Controllers.Volunteers.Requests;

public readonly record struct SignUpForVolunteerRequest(
    string ApplicantName,
    int ApplicantAge,
    string? ApplicantPhoneNumber,
    string? ApplicantEmail,
    CommunicationMethod CommunicationMethod
);

public sealed partial class SignUpForVolunteerRequestValidator : AbstractValidator<SignUpForVolunteerRequest>
{
    [GeneratedRegex("([А-Я]|[а-я]|[A-Z]|[a-z])+")]
    private partial Regex ApplicantNameRegex();


    [GeneratedRegex(@"\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}")]
    private partial Regex PhoneNumberRegex();

    public SignUpForVolunteerRequestValidator()
    {
        RuleFor(x => x.ApplicantName)
            .NotEmpty()
            .Must(x => x.Split(' ').Length == 2)
            .Must(x => ApplicantNameRegex().IsMatch(x));

        RuleFor(x => x.ApplicantAge)
            .GreaterThanOrEqualTo(18)
            .LessThanOrEqualTo(122);

        RuleFor(x => x.ApplicantPhoneNumber)
            .NotEmpty()
            .When(x => x.CommunicationMethod.HasFlag(CommunicationMethod.CallAPhone) ||
                       x.CommunicationMethod.HasFlag(CommunicationMethod.WhatsApp) ||
                       x.CommunicationMethod.HasFlag(CommunicationMethod.Telegram))
            .Must(x => PhoneNumberRegex().IsMatch(x!))
            .When(x => x.CommunicationMethod.HasFlag(CommunicationMethod.CallAPhone) ||
                       x.CommunicationMethod.HasFlag(CommunicationMethod.WhatsApp) ||
                       x.CommunicationMethod.HasFlag(CommunicationMethod.Telegram));

        RuleFor(x => x.ApplicantEmail)
            .NotEmpty()
            .When(x => x.CommunicationMethod.HasFlag(CommunicationMethod.Email))
            .EmailAddress()
            .When(x => x.CommunicationMethod.HasFlag(CommunicationMethod.Email));
    }
}