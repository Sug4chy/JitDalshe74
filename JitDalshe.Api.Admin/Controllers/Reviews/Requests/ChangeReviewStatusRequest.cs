using System.Text.Json.Serialization;
using FluentValidation;
using JitDalshe.Api.Attributes;
using JitDalshe.Domain.Entities.Reviews;

namespace JitDalshe.Api.Admin.Controllers.Reviews.Requests;

[Validator<ChangeReviewStatusRequestValidator>]
public sealed record ChangeReviewStatusRequest(
    [property: JsonConverter(typeof(JsonStringEnumConverter))] 
    ReviewStatus Status
);

public sealed class ChangeReviewStatusRequestValidator : AbstractValidator<ChangeReviewStatusRequest>
{
    public ChangeReviewStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}