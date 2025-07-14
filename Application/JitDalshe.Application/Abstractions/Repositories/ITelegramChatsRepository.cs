using JitDalshe.Application.Entities;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface ITelegramChatsRepository
{
    Task AddAsync(TelegramChat chat, CancellationToken ct = default);
}