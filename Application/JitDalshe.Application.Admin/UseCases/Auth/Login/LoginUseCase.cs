using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Abstractions.Security;
using JitDalshe.Application.Attributes;

namespace JitDalshe.Application.Admin.UseCases.Auth.Login;

[UseCase]
internal sealed class LoginUseCase : ILoginUseCase
{
    private readonly IAdminUsersRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginUseCase(IAdminUsersRepository users, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<LoginResult> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        try
        {
            var maybeUser = await _users.FindByEmailAsync(email, ct);
            if (maybeUser.HasNoValue || !maybeUser.Value.IsActive)
            {
                return LoginResult.Failed;
            }

            var user = maybeUser.Value;
            if (!_passwordHasher.Verify(password, user.PasswordHash))
            {
                return LoginResult.Failed;
            }

            var token = _jwtProvider.GenerateToken(user.Id, user.Email, user.Role);
            return LoginResult.Ok(token);
        }
        catch (Exception e)
        {
            return LoginResult.Failure(e.Message);
        }
    }
}