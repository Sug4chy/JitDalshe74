using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace JitDalshe.Infrastructure.Telegram.Initialization;

internal sealed class SetupBotAsyncInitializer : IAsyncInitializer
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IHostEnvironment _environment;
    private readonly string _webhookUrl;
    private readonly string _certificatePath;

    public SetupBotAsyncInitializer(
        ITelegramBotClient telegramBotClient,
        IHostEnvironment environment,
        string webhookUrl,
        string certificatePath)
    {
        _telegramBotClient = telegramBotClient;
        _environment = environment;
        _webhookUrl = webhookUrl;
        _certificatePath = certificatePath;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        // Чтобы при локальном запуске с Tuna не было ошибок от отсутствия сертификата
        if (_environment.IsDevelopment())
        {
            await _telegramBotClient.SetWebhook(
                url: _webhookUrl,
                allowedUpdates: Update.AllTypes,
                cancellationToken: cancellationToken
            );
        }
        else
        {
            var fileStream = File.OpenRead(_certificatePath);

            await _telegramBotClient.SetWebhook(
                url: _webhookUrl,
                allowedUpdates: Update.AllTypes,
                certificate: new InputFileStream(fileStream),
                cancellationToken: cancellationToken
            );
        }
    }
}