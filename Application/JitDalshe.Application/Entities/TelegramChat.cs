using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Entities;

public sealed class TelegramChat : AuditableEntity<IdOf<TelegramChat>>
{
    public long ExtId { get; }

    private TelegramChat(IdOf<TelegramChat> id, long extId)
    {
        Id = id;
        ExtId = extId;
    }

    public static TelegramChat Create(IdOf<TelegramChat> id, long extId) => new(id, extId);

    /// <summary>
    /// For EF Core
    /// </summary>
    [UsedImplicitly]
#pragma warning disable CS8618
    private TelegramChat()
    {
    }
#pragma warning restore CS8618
}