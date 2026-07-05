using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Users;

public sealed class AdminUser : AuditableEntity<IdOf<AdminUser>>
{
    public string Email { get; init; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }

    private AdminUser(
        IdOf<AdminUser> id,
        string email,
        string passwordHash,
        UserRole role,
        bool isActive)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = isActive;
    }

    public static AdminUser Create(
        IdOf<AdminUser> id,
        string email,
        string passwordHash,
        UserRole role = UserRole.Standard)
        => new(id, email, passwordHash, role, true);

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }

    public void ChangeRole(UserRole newRole)
    {
        if (Role == UserRole.SuperAdmin && newRole != UserRole.SuperAdmin)
        {
            throw new InvalidOperationException("Нельзя понизить суперадмина.");
        }
        Role = newRole;
    }

    public void Deactivate()
    {
        if (Role == UserRole.SuperAdmin)
        {
            throw new InvalidOperationException("Роль суперадмина нельзя деактивировать");
        }
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    [UsedImplicitly]
#pragma warning disable CS8618
    private AdminUser()
    {
    }
#pragma warning restore CS8618
}