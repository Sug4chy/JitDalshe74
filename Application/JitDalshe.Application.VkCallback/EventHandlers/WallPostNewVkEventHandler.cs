using System.Text.Json;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.VkCallback.Abstractions;
using JitDalshe.Application.VkCallback.Events;
using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace JitDalshe.Application.VkCallback.EventHandlers;

public sealed class WallPostNewVkEventHandler : IVkEventHandler
{
    private static readonly List<string> SizeTypesSorted = ["s", "m", "x", "o", "p", "q", "r", "y", "z", "w"];

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly INewsRepository _newsRepository;
    private readonly ILogger<WallPostNewVkEventHandler> _logger;

    public string EventType => "wall_post_new";

    public WallPostNewVkEventHandler(INewsRepository newsRepository, ILogger<WallPostNewVkEventHandler> logger)
    {
        _newsRepository = newsRepository;
        _logger = logger;
    }

    public Task HandleAsync(VkEvent @event, CancellationToken ct = default)
    {
        var payload = @event.Object.Deserialize<WallPostNewVkEventPayload>(JsonSerializerOptions);
        if (payload is not null)
        {
            return HandleAsync(payload, @event.GroupId, ct);
        }

        _logger.LogError("Cannot deserialize wall_post_new vk event payload");
        return Task.CompletedTask;
    }

    private async Task HandleAsync(
        WallPostNewVkEventPayload payload,
        long groupId, 
        CancellationToken ct = default)
    {
        var photoAttachments = payload.Attachments.Where(x => x.Type is "photo");
        var newsImages = photoAttachments
            .Select(x => NewsImage.Create(
                id: IdOf<NewsImage>.New(),
                extId: x.Photo!.Id,
                url: new Uri(x.Photo.Sizes.MaxBy(s => SizeTypesSorted.IndexOf(s.Type))!.Url))
            )
            .ToList();
        
        var news = News.Create(
            id: IdOf<News>.New(),
            extId: payload.Id,
            text: payload.Text,
            publicationDate: DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(payload.Date).Date),
            postUrl: $"https://vk.com/wall{groupId}_{payload.Id}",
            isDisplaying: false,
            images: newsImages);

        if (news.Images.Count is not 0)
        {
            news.PrimaryImage = NewsPrimaryImage.Create(
                newsId: news.Id,
                newsImageId: news.Images.First().Id);
        }
        
        await _newsRepository.AddAsync(news, ct);
    }
}