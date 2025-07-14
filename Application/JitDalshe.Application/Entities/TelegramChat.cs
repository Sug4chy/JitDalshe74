using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Entities;

public sealed class TelegramChat : AuditableEntity<IdOf<TelegramChat>>
{
    public long ExtId { get; }

    private TelegramChat(long extId)
    {
        ExtId = extId;
    }

    public static TelegramChat Create(long extId) => new(extId);

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