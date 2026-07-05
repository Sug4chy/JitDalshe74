using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Extensions;
using JitDalshe.Infrastructure.Persistence.Repositories.Base;

namespace JitDalshe.Infrastructure.Persistence.Repositories;


[Repository]
internal sealed class AdminUsersRepository(PostgresqlDbContext dbContext)
    : BaseRepository<AdminUser>(dbContext), IAdminUsersRepository
{
    public Task<Maybe<AdminUser>> FindByEmailAsync(string email, CancellationToken ct = default)
        => DbContext.AdminUsers.TryFirstAsync(x => x.Email == email, ct);
}