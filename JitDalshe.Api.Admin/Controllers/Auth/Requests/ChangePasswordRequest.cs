using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.Auth.Requests;

[Validator<ChangePasswordRequestValidator>]
public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);

public sealed class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6).WithMessage("Новый пароль должен быть не менее 6 символов");
    }
}