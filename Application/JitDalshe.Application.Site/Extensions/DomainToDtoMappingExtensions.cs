using JitDalshe.Application.Site.Dto;
using JitDalshe.Application.Site.Dto.Events;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.Entities.Reviews;

namespace JitDalshe.Application.Site.Extensions;

public static class DomainToDtoMappingExtensions
{
    public static NewsDto ToDto(this News news)
        => new(news.Text, news.PrimaryImage?.NewsImage!.Url.ToString() ?? string.Empty, news.PostUrl);

    public static EventPreviewDto ToPreviewDto(this Event @event)
        => new(@event.Id, @event.ShortDescription, @event.Image!.Url, @event.Date);

    public static EventDto ToDto(this Event @event)
        => new(
            @event.Id,
            @event.Title,
            @event.ShortDescription,
            @event.FullText,
            @event.Date,
            @event.Time,
            @event.Location,
            @event.Image!.Url
        );
    
    public static ReviewDto ToDto(this Review review)
        => new(
            ReviewerName: review.ReviewerName, 
            ReviewerAge: review.ReviewerAge, 
            ReviewerStatus: review.ReviewerStatus, 
            Text: review.Text, 
            Date: DateOnly.FromDateTime(review.CreatedAt.Date));
}