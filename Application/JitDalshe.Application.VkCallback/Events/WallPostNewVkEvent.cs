namespace JitDalshe.Application.VkCallback.Events;

public sealed record WallPostNewVkEventPayload(
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