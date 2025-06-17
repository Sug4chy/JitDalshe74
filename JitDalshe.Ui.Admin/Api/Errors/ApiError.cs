using System.Text.Json.Serialization;

namespace JitDalshe.Ui.Admin.Api.Errors;

public readonly struct ApiError
{
    [JsonPropertyName("message")]
    public string Message { get; init; }
}