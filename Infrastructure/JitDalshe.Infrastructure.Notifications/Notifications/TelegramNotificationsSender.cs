using JitDalshe.Application.Abstractions.Notifications;
using JitDalshe.Application.Abstractions.Repositories;
using Telegram.Bot;

namespace JitDalshe.Infrastructure.Notifications.Notifications;

internal sealed class TelegramTelegramNotificationsSender : ITelegramNotificationsSender
{
    private readonly ITelegramChatsRepository _telegramChats;
    private readonly ITelegramBotClient _telegramBot;

    public TelegramTelegramNotificationsSender(ITelegramChatsRepository telegramChats, ITelegramBotClient telegramBot)
    {
        _telegramChats = telegramChats;
        _telegramBot = telegramBot;
    }

    public async Task SendAsync(string text, CancellationToken ct = default)
    {
        var chats = await _telegramChats.FindAllAsync(ct);
        foreach (var chat in chats)
        {
            await _telegramBot.SendMessage(chat.ExtId, text, cancellationToken: ct);
        }
    }
}