namespace JitDalshe.Application.Abstractions.Notifications;

public interface ITelegramNotificationsSender
{
    Task SendAsync(string text, CancellationToken ct = default);
}