using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Entities;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Repositories;

[Repository]
internal sealed class TelegramChatsRepository : ITelegramChatsRepository
{
    private readonly PostgresqlDbContext _db;

    public TelegramChatsRepository(PostgresqlDbContext db)
    {
        _db = db;
    }

    public Task<TelegramChat[]> FindAllAsync(CancellationToken ct = default)
        => _db.TelegramChats.ToArrayAsync(ct);

    public async Task AddAsync(TelegramChat chat, CancellationToken ct = default)
    {
        _db.TelegramChats.Add(chat);
        await _db.SaveChangesAsync(ct);
    }
}