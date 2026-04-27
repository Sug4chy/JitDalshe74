using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.SupportGroups.Requests;

[Validator<UpdateSupportGroupCommentRequestValidator>]
public sealed record UpdateSupportGroupCommentRequest(
    string? Comment
);

public sealed class UpdateSupportGroupCommentRequestValidator : AbstractValidator<UpdateSupportGroupCommentRequest>
{
    public UpdateSupportGroupCommentRequestValidator()
    {
        RuleFor(x => x.Comment)
            .MaximumLength(1000).WithMessage("Comment length must be no more than 1000 characters long");
    }
}