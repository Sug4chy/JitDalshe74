using JitDalshe.Application.Admin.Dto;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.Entities.News;

namespace JitDalshe.Application.Admin.Extensions;

public static class DomainToDtoMappingExtensions
{
    public static NewsDto ToDto(this News news)
        => new(
            Id: news.Id,
            Text: news.Text,
            PublishDate: news.PublicationDate,
            Images: news.Images
                .Select(x => x.ToDto(news.PrimaryImage is not null && x.Id == news.PrimaryImage.NewsImageId))
                .ToArray(),
            PostUrl: news.PostUrl);

    private static NewsImageDto ToDto(this NewsImage newsImage, bool isPrimary)
        => new(
            Id: newsImage.Id,
            Url: newsImage.Url.ToString(),
            IsPrimary: isPrimary);

    public static EventDto ToDto(this Event @event)
        => new(
            Id: @event.Id,
            Title: @event.Title,
            Description: @event.Description,
            Date: @event.Date,
            ImageUrl: @event.Image!.Url
        );

    public static BannerDto ToDto(this Banner banner)
        => new(
            Id:  banner.Id,
            Title: banner.Title,
            RedirectOnClickUrl: banner.RedirectOnClickUrl,
            DisplayOrder: banner.DisplayOrder,
            ImageUrl: banner.Image!.Url
        );
}