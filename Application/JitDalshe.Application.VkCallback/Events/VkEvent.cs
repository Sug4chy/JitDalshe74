using System.Text.Json;
using System.Text.Json.Serialization;

namespace JitDalshe.Application.VkCallback.Events;

public sealed record VkEvent(
    string Type,

    [property:JsonPropertyName("event_id")]
    long EventId,
    string V,

    [property:JsonPropertyName("group_id")]
    long GroupId,

    JsonElement Object
);