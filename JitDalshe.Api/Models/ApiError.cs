using JitDalshe.Application.Errors;

namespace JitDalshe.Api.Models;

public readonly struct ApiError
{
    public required string Message { get; init; }

    public static ApiError From(Error error) => new() { Message = error.Message };

    public static ApiError From(string message) => new() { Message = message };
}