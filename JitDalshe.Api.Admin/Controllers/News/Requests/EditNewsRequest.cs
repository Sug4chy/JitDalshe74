using FluentValidation;
using JitDalshe.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.News.Requests;

[Validator<EditNewsRequestValidator>]
public sealed record EditNewsRequest(
    string Text,
    Guid PrimaryPhotoId
);

public sealed class EditNewsRequestValidator : AbstractValidator<EditNewsRequest>
{
    public EditNewsRequestValidator()
    {
        RuleFor(x => x.Text).NotEmpty();
        RuleFor(x => x.PrimaryPhotoId).NotEmpty();
    }
}