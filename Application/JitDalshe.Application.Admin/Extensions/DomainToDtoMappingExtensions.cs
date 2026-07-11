using JitDalshe.Application.Admin.Dto;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.Entities.SupportGroups;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.Entities.Volunteers;

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
            PostUrl: news.PostUrl,
            IsDisplaying: news.IsDisplaying);

    private static NewsImageDto ToDto(this NewsImage newsImage, bool isPrimary)
        => new(
            Id: newsImage.Id,
            Url: newsImage.Url.ToString(),
            IsPrimary: isPrimary);

    public static EventDto ToDto(this Event @event)
        => new(
            Id: @event.Id,
            Title: @event.Title,
            ShortDescription: @event.ShortDescription,
            FullText: @event.FullText,
            Date: @event.Date,
            Time: @event.Time,
            Location: @event.Location,
            Status: @event.Status,
            ImageUrl: @event.Image!.Url
        );

    public static BannerDto ToDto(this Banner banner)
        => new(
            Id: banner.Id,
            Title: banner.Title,
            Description: banner.Description,
            RedirectOnClickUrl: banner.RedirectOnClickUrl,
            DisplayOrder: banner.DisplayOrder,
            Status: banner.Status,
            ImageUrl: banner.Image!.Url,
            MobileImageUrl: banner.MobileImage!.Url
        );

    public static ReviewDto ToDto(this Review review)
        => new(
            Id: review.Id,
            ReviewerName: review.ReviewerName,
            ReviewerAge: review.ReviewerAge,
            ReviewerStatus: review.ReviewerStatus,
            Text: review.Text,
            Status: review.Status,
            Date: DateOnly.FromDateTime(review.CreatedAt)
        );
    
    public static ConsultationRequestDto ToDto(this ConsultationRequest request)
        => new(
            Id: request.Id,
            PatientName: request.PatientName,
            PatientAge: request.PatientAge,
            PatientPhoneNumber: request.PatientPhoneNumber,
            PatientEmail: request.PatientEmail,
            Status: request.ConsultationRequestStatus,
            CommunicationMethods: request.CommunicationMethods,
            Date: DateOnly.FromDateTime(request.CreatedAt),
            Comment: request.Comment
        );
    
    public static SupportGroupRequestDto ToDto(this SupportGroupRequest request)
        => new(
            Id: request.Id,
            ApplicantName: request.ApplicantName,
            ApplicantAge: request.ApplicantAge,
            ApplicantPhoneNumber: request.ApplicantPhoneNumber,
            ApplicantEmail: request.ApplicantEmail,
            Status: request.Status,
            CommunicationMethods: request.CommunicationMethods,
            Date: DateOnly.FromDateTime(request.CreatedAt),
            Comment: request.Comment
        );
    
    public static VolunteerRequestDto ToDto(this VolunteerRequest request)
        => new(
            Id: request.Id,
            ApplicantName: request.ApplicantName,
            ApplicantAge: request.ApplicantAge,
            ApplicantPhoneNumber: request.ApplicantPhoneNumber,
            ApplicantEmail: request.ApplicantEmail,
            Status: request.Status,
            CommunicationMethods: request.CommunicationMethods,
            Date: DateOnly.FromDateTime(request.CreatedAt),
            Comment: request.Comment
        );

    public static AdminUserDto ToDto(this AdminUser user)
        => new(
            Id: user.Id,
            Email: user.Email,
            Role: user.Role,
            IsActive: user.IsActive,
            CreatedAt: DateOnly.FromDateTime(user.CreatedAt)
        );
}