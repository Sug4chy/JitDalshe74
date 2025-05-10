using JitDalshe.Application.Site.Dto;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.Entities.News;

namespace JitDalshe.Application.Site.Extensions;

public static class DomainToDtoMappingExtensions
{
    public static NewsDto ToDto(this News news)
        => new(news.Text, news.PrimaryImage?.NewsImage!.Url.ToString() ?? string.Empty);

    public static EventDto ToDto(this Event @event)
        => new(@event.Title, @event.Image!.Url);
}