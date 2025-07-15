namespace JitDalshe.Application.Abstractions.Notifications;

public interface INotificationsSender
{
    Task SendAsync(string text, CancellationToken ct = default);
}