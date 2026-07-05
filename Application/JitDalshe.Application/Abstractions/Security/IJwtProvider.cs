using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.Security;

public interface IJwtProvider
{
    string GenerateToken(IdOf<AdminUser> userId, string email, UserRole role);
}