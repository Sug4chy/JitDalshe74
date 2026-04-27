using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.Volunteers.Requests;

[Validator<UpdateVolunteerCommentRequestValidator>]
public sealed record UpdateVolunteerCommentRequest(
    string? Comment
);

public sealed class UpdateVolunteerCommentRequestValidator : AbstractValidator<UpdateVolunteerCommentRequest>
{
    public UpdateVolunteerCommentRequestValidator()
    {
        RuleFor(x => x.Comment)
            .MaximumLength(1000).WithMessage("Comment length must be no more than 1000 characters long");
    }
}