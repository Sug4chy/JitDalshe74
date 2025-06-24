using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Errors;
using JitDalshe.Application.VkCallback.Abstractions;
using JitDalshe.Application.VkCallback.Events;
using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace JitDalshe.Application.VkCallback.EventHandlers;

public sealed class WallPostNewVkEventHandler : IVkEventHandler<WallPostNewVkEvent>
{
    private static readonly List<string> SizeTypesSorted = ["s", "m", "x", "o", "p", "q", "r", "y", "z", "w"];

    private readonly INewsRepository _newsRepository;
    private readonly ILogger<WallPostNewVkEventHandler> _logger;

    public WallPostNewVkEventHandler(INewsRepository newsRepository, ILogger<WallPostNewVkEventHandler> logger)
    {
        _newsRepository = newsRepository;
        _logger = logger;
    }

    public async Task<UnitResult<Error>> HandleAsync(WallPostNewVkEvent @event, CancellationToken ct)
    {
        try
        {
            var photoAttachments = @event.Object.Attachments.Where(x => x.Type is "photo");
            var newsPhotos = photoAttachments
                .Select(x => NewsImage.Create(
                    id: IdOf<NewsImage>.New(),
                    extId: x.Photo!.Id,
                    url: new Uri(x.Photo.Sizes.MaxBy(s => SizeTypesSorted.IndexOf(s.Type))!.Url))
                )
                .ToList();

            var news = News.Create(
                id: IdOf<News>.New(),
                extId: @event.Object.Id,
                text: @event.Object.Text,
                publicationDate: DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(@event.Object.Date).Date),
                postUrl: $"https://vk.com/wall{@event.GroupId}_{@event.Object.Id}",
                isDisplaying: false,
                images: newsPhotos);

            if (news.Images.Count is not 0)
            {
                news.PrimaryImage = NewsPrimaryImage.Create(
                    newsId: news.Id,
                    newsImageId: news.Images.First().Id);
            }

            await _newsRepository.AddAsync(news, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            _logger.LogError(exception: e, message: "An error occured while adding new post");
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}