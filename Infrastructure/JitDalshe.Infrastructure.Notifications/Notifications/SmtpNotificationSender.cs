
using JitDalshe.Application.Abstractions.Notifications;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace JitDalshe.Infrastructure.Notifications.Notifications;

public sealed class SmtpNotificationSender(IConfiguration configuration, ILogger<SmtpNotificationSender> logger) : IEmailNotificationsSender
{
    public async Task SendAsync(string subject, string body, CancellationToken ct = default)
    {
        try
        {
            var host = configuration["Email:SmtpServer"];
            var portStr = configuration["Email:Port"];
            var username = configuration["Email:Username"];
            var password = configuration["Email:Password"];
            var from = configuration["Email:SenderEmail"];
            var to = configuration["Email:RecipientEmail"];

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(to))
            {
                logger.LogWarning("SMTP MailKit sender is not fully configured. Skipping email notification.");
                return;
            }

            int port = int.TryParse(portStr, out var parsedPort) ? parsedPort : 587;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("АНО «Жить Дальше»", from));
            message.To.Add(new MailboxAddress("Администратор", to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            var socketOption = port == 465 
                ? SecureSocketOptions.SslOnConnect 
                : SecureSocketOptions.StartTls;

            await client.ConnectAsync(host, port, socketOption, ct);
            await client.AuthenticateAsync(username, password, ct);
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);

            logger.LogInformation("Notification email successfully sent to {Recipient} via MailKit", to);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send notification email via MailKit: {Message}", ex.Message);
        }
    }
}