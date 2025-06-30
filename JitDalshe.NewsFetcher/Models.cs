namespace JitDalshe.NewsFetcher;

public sealed record VkWallGetResponse(
    int Count,
    VkWallPost[] Items
);

public sealed record VkWallPost(
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