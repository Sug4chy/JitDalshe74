using OneOf;
using static JitDalshe.Application.Site.UseCases.Volunteers.SignUpForVolunteerResult;

namespace JitDalshe.Application.Site.UseCases.Volunteers;

[GenerateOneOf]
public sealed partial class SignUpForVolunteerResult : OneOfBase<SignedUp, Error>
{
    public sealed class SignedUp;
    public static SignedUp Success() => new();

    public sealed class Error
    {
        public required string Message { get; init; }
    }
    public static Error Failed(string message) => new() { Message = message };
}