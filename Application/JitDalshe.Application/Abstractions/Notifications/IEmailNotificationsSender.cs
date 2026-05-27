namespace JitDalshe.Application.Abstractions.Notifications;

public interface IEmailNotificationsSender
{
    Task SendAsync(string subject, string body, CancellationToken ct = default);
}