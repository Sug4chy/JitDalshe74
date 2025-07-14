using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Entities;
using JitDalshe.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace JitDalshe.Application.TelegramBot;

public sealed class UpdatesHandler
{
    private readonly ILogger<UpdatesHandler> _logger;
    private readonly ITelegramChatsRepository _telegramChats;

    public UpdatesHandler(ILogger<UpdatesHandler> logger, ITelegramChatsRepository telegramChats)
    {
        _logger = logger;
        _telegramChats = telegramChats;
    }

    public async Task HandleAsync(Update update, CancellationToken ct = default)
    {
        try
        {
            if (update is not { Type: UpdateType.Message, Message: { Text: not null, Chat.Type: ChatType.Private } })
            {
                _logger.LogWarning("Unhandled update: [{Type}] - {ID}", update.Type.ToString(), update.Id);
                return;
            }

            if (!update.Message.Text.StartsWith("/start"))
            {
                _logger.LogWarning("Unknown input: [{Type}] - {ID} ({Text})",
                    update.Type.ToString(), update.Id, update.Message.Text);
                return;
            }

            await _telegramChats.AddAsync(TelegramChat.Create(IdOf<TelegramChat>.New(), update.Message.Chat.Id), ct);
            _logger.LogInformation("Saved new chat with ID {ID}", update.Message.Chat.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception was raised: {Message}", e.Message);
        }
    }
}