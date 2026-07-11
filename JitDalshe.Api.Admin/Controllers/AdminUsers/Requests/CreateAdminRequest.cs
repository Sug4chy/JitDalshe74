using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.AdminUsers.Requests;

[Validator<CreateAdminRequestValidator>]
public sealed record CreateAdminRequest(string Email, string Password);

public sealed class CreateAdminRequestValidator : AbstractValidator<CreateAdminRequest>
{
    public CreateAdminRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Адрес электронной почты указан неверно");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Пароль должен быть не менее 6 символов");
    }
}