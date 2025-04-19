using JitDalshe.Application.VkCallback.Abstractions;

namespace JitDalshe.Application.VkCallback.Events;

public record WallPostNewVkEvent(
    string Type,
    long EventId,
    string V,
    long GroupId,
    NewsCreatedEventPayload Object
) : VkEvent(Type, EventId, V, GroupId);

public sealed record NewsCreatedEventPayload(
    long Id,
    long Date,
    string Text,
    Attachment[] Attachments
);

public sealed record Attachment(
    string Type,
    Photo? Photo = null
);

public sealed record Photo(
    long Id,
    PhotoSize[] Sizes
);

public sealed record PhotoSize(
    string Type,
    string Url
);