using FluentValidation;
using JitDalshe.Api.Attributes;
using JitDalshe.Domain.Entities.Reviews;

namespace JitDalshe.Api.Site.Controllers.Reviews.Requests;

[Validator<LeaveReviewRequestValidator>]
public readonly record struct LeaveReviewRequest(
    string ReviewerName,
    int ReviewerAge,
    ReviewerStatus ReviewerStatus,
    string Text
);

public sealed class LeaveReviewRequestValidator : AbstractValidator<LeaveReviewRequest>
{
    public LeaveReviewRequestValidator()
    {
        RuleFor(x => x.ReviewerName).NotEmpty();
        RuleFor(x => x.ReviewerAge).GreaterThan(0).LessThan(110);
        RuleFor(x => x.Text).NotEmpty();
    }
}