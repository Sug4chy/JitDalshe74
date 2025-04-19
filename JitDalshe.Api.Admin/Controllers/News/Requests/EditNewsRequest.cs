using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.News.Requests;

[Validator<EditNewsRequestValidator>]
public sealed record EditNewsRequest(
    string Text,
    Guid? PrimaryPhotoId = null
);

public sealed class EditNewsRequestValidator : AbstractValidator<EditNewsRequest>
{
    public EditNewsRequestValidator()
    {
        RuleFor(x => x.Text).NotEmpty();
    }
}