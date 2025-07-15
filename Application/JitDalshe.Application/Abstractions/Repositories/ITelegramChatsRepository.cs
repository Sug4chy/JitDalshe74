using JitDalshe.Application.Entities;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface ITelegramChatsRepository
{
    Task<TelegramChat[]> FindAllAsync(CancellationToken ct = default);
    Task AddAsync(TelegramChat chat, CancellationToken ct = default);
}