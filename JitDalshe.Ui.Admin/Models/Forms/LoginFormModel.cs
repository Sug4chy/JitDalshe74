using FluentValidation;

namespace JitDalshe.Ui.Admin.Models.Forms;

public sealed class LoginFormModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public sealed class LoginFormModelValidator : AbstractValidator<LoginFormModel>
{
    public LoginFormModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Пожалуйста, введите адрес электронной почты")
            .EmailAddress().WithMessage("Неверный формат адреса электронной почты");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пожалуйста, введите пароль");
    }
}