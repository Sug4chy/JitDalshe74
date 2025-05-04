using System.Text.Json.Serialization;

namespace JitDalshe.Ui.Admin.Api;

public readonly struct ApiValidationError
{
    [JsonPropertyName("errors")]
    public Dictionary<string, string[]> Errors { get; init; }
}