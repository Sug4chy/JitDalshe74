namespace JitDalshe.Application.Models;

public sealed record ImageModel(
    Stream ImageStream,
    string ContentType
);