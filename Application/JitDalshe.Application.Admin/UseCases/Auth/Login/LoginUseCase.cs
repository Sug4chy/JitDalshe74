using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Abstractions.Security;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.Auth.Login;

[UseCase]
internal sealed class LoginUseCase(
    IAdminUsersRepository users,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider)
    : ILoginUseCase
{
    public async Task<Result<string, Error>> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        try
        {
            var maybeUser = await users.FindByEmailAsync(email, ct);
            if (maybeUser.HasNoValue || !maybeUser.Value.IsActive)
            {
                return Result.Failure<string, Error>(Error.Of("Неверный email или пароль", ErrorGroup.Unauthorized));
            }

            var user = maybeUser.Value;
            if (!passwordHasher.Verify(password, user.PasswordHash))
            {
                return Result.Failure<string, Error>(Error.Of("Неверный email или пароль", ErrorGroup.Unauthorized));
            }

            var token = jwtProvider.GenerateToken(user.Id, user.Email, user.Role);
            return Result.Success<string, Error>(token);
        }
        catch (Exception e)
        {
            return Result.Failure<string, Error>(Error.Of(e.Message, ErrorGroup.InternalError));
        }
    }
}