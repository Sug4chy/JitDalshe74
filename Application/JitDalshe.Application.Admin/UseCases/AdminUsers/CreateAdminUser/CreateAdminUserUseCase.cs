using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Abstractions.Security;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.AdminUsers.CreateAdminUser;

[UseCase]
internal sealed class CreateAdminUserUseCase(IAdminUsersRepository repository, IPasswordHasher passwordHasher)
    : ICreateAdminUserUseCase
{
    public async Task<UnitResult<Error>> CreateAsync(string email, string password, CancellationToken ct = default)
    {
        try
        {
            var maybeExisting = await repository.FindByEmailAsync(email, ct);
            if (maybeExisting.HasValue)
            {
                return UnitResult.Failure(Error.Of("Администратор с такой почтой уже существует"));
            }

            var passwordHash = passwordHasher.Hash(password);
            var newUser = AdminUser.Create(IdOf<AdminUser>.New(), email, passwordHash);

            await repository.AddAsync(newUser, ct);
            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}