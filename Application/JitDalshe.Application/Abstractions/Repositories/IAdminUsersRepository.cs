using CSharpFunctionalExtensions;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface IAdminUsersRepository
{
    Task<Maybe<AdminUser>> FindByIdAsync(IdOf<AdminUser> id,  CancellationToken ct = default);
    Task<Maybe<AdminUser>> FindByEmailAsync(string email, CancellationToken ct = default);
    Task AddAsync(AdminUser user, CancellationToken ct = default);
    Task EditAsync(AdminUser user, CancellationToken ct = default);
}