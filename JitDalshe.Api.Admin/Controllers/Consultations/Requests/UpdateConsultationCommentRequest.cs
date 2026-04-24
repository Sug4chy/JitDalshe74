using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.Consultations.Requests;

[Validator<UpdateConsultationCommentRequestValidator>]
public sealed record UpdateConsultationCommentRequest(
    string? Comment
);

public sealed class UpdateConsultationCommentRequestValidator : AbstractValidator<UpdateConsultationCommentRequest>
{
    public UpdateConsultationCommentRequestValidator()
    {
        RuleFor(x => x.Comment)
            .MaximumLength(1000).WithMessage("Comment length must be no more than 1000 characters long");
    }
}