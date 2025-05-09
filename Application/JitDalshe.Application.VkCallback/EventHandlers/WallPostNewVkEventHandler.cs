using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Errors;
using JitDalshe.Application.VkCallback.Abstractions;
using JitDalshe.Application.VkCallback.Events;
using JitDalshe.Domain.Entities.News;
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
                .Select(x => new NewsPhoto
                {
                    ExtId = x.Photo!.Id,
                    Uri = new Uri(
                        x.Photo.Sizes.MaxBy(s => SizeTypesSorted.IndexOf(s.Type))!.Url)
                })
                .ToList();

            var news = new News
            {
                ExtId = @event.Object.Id,
                Text = @event.Object.Text,
                PublicationDate = DateOnly.FromDateTime(
                    DateTimeOffset.FromUnixTimeSeconds(@event.Object.Date).Date),
                Photos = newsPhotos,
            };
            if (news.Photos.Count is not 0)
            {
                news.PrimaryPhoto = new NewsPrimaryPhoto
                {
                    NewsId = news.Id,
                    NewsPhotoId = news.Photos.First().Id
                };
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