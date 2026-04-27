using static JitDalshe.Application.Site.UseCases.SupportGroups.SignUpForSupportGroupResult;
using OneOf;

namespace JitDalshe.Application.Site.UseCases.SupportGroups;

[GenerateOneOf]
public sealed partial class SignUpForSupportGroupResult : OneOfBase<SignedUp, Error>
{
    public sealed class SignedUp;
    
    public static SignedUp Success() => new();

    public sealed class Error
    {
        public required string Message { get; init; }
    }
    public static Error Failed(string message) => new() { Message = message };
}