using OneOf;

namespace JitDalshe.Application.Admin.UseCases.Auth.Login;

[GenerateOneOf]
public sealed partial class LoginResult : OneOfBase<LoginResult.Success, LoginResult.InvalidCredentials, LoginResult.Error>
{
    public sealed class Success
    {
        public required string Token { get; init; }
    }
    public static Success Ok(string token) => new() { Token = token };

    public sealed class InvalidCredentials;
    public static readonly InvalidCredentials Failed = new();

    public sealed class Error
    {
        public required string Message { get; init; }
    }
    public static Error Failure(string message) => new() { Message = message };
}