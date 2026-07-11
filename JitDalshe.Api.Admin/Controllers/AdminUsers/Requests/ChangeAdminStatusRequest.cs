using FluentValidation;
using JitDalshe.Api.Attributes;

namespace JitDalshe.Api.Admin.Controllers.AdminUsers.Requests;

[Validator<ChangeAdminStatusRequestValidator>]
public sealed record ChangeAdminStatusRequest(bool IsActive);

public sealed class ChangeAdminStatusRequestValidator : AbstractValidator<ChangeAdminStatusRequest>
{
    public ChangeAdminStatusRequestValidator()
    {
        RuleFor(x => x.IsActive).NotNull();
    }
}