namespace JitDalshe.Application.Models;

public sealed record EventImageModel(
    Stream ImageStream,
    string ContentType
);