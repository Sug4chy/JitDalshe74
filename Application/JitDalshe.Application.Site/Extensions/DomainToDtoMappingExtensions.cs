using JitDalshe.Application.Site.Dto;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.Entities.Reviews;

namespace JitDalshe.Application.Site.Extensions;

public static class DomainToDtoMappingExtensions
{
    public static NewsDto ToDto(this News news)
        => new(news.Text, news.PrimaryImage?.NewsImage!.Url.ToString() ?? string.Empty, news.PostUrl);

    public static EventDto ToDto(this Event @event)
        => new(@event.Title, @event.Image!.Url, @event.Date);

    public static ReviewDto ToDto(this Review review)
        => new(
            ReviewerName: review.ReviewerName, 
            ReviewerAge: review.ReviewerAge, 
            ReviewerStatus: review.ReviewerStatus, 
            Text: review.Text, 
            Date: DateOnly.FromDateTime(review.CreatedAt.Date));
}