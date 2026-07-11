using FluentValidation;

namespace JitDalshe.Ui.Admin.Models.Forms;

public sealed class CreateAdminUserFormModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public sealed class CreateAdminUserFormModelValidator : AbstractValidator<CreateAdminUserFormModel>
{
    public CreateAdminUserFormModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Укажите адрес электронной почты")
            .EmailAddress().WithMessage("Некорректный формат электронной почты");
            
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Укажите пароль")
            .MinimumLength(6).WithMessage("Пароль должен содержать минимум 6 символов");
    }
}