using System.Text.Json.Serialization;
using FluentValidation;
using JitDalshe.Api.Attributes;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Consultations;

namespace JitDalshe.Api.Admin.Controllers.Consultations.Requests;

[Validator<ChangeConsultationRequestStatusRequestValidator>]
public sealed record ChangeConsultationRequestStatusRequest(
    [property: JsonConverter(typeof(JsonStringEnumConverter))] 
    RequestStatus Status
);

public sealed class ChangeConsultationRequestStatusRequestValidator : AbstractValidator<ChangeConsultationRequestStatusRequest>
{
    public ChangeConsultationRequestStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}