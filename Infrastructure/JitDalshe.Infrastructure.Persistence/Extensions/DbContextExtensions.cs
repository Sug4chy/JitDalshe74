using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.Entities.SupportGroups;
using JitDalshe.Domain.Entities.Volunteers;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Extensions;

public static class DbContextExtensions
{
    public static async Task SeedDataAsync(this PostgresqlDbContext context)
    {
        bool hasChanges = false;

        hasChanges |= await context.SeedBannersAsync();
        hasChanges |= await context.SeedEventsAsync();
        hasChanges |= await context.SeedReviewsAsync();
        hasChanges |= await context.SeedConsultationRequestsAsync();
        hasChanges |= await context.SeedSupportGroupRequestsAsync();
        hasChanges |= await context.SeedVolunteerRequestsAsync();
        
        if (hasChanges)
        {
            await context.SaveChangesAsync();
        }
    }
    
    private static async Task<bool> SeedBannersAsync(this PostgresqlDbContext context)
    {
        if (await context.Banners.AnyAsync()) return false;

        var bannerId = IdOf<Banner>.New();
        var bannerImage = BannerImage.Create(
            id: IdOf<BannerImage>.New(),
            url: "https://via.placeholder.com/1200x400.png?text=Test+Banner+Desktop",
            contentType: "image/png",
            bannerId: bannerId);
        
        var bannerMobileImage = BannerMobileImage.Create(
            id: IdOf<BannerMobileImage>.New(),
            url: "https://via.placeholder.com/600x400.png?text=Test+Banner+Mobile",
            contentType: "image/png",
            bannerId: bannerId);

        var banner = Banner.Create(
            id: bannerId,
            title: "Тестовый баннер",
            description: "Это демонстрационный баннер, созданный автоматически.",
            redirectOnClickUrl: "https://vk.com/zhitdalshe74",
            displayOrder: 1,
            image: bannerImage,
            mobileImage: bannerMobileImage,
            status: BannerStatus.Published
        );

        context.Banners.Add(banner);
        context.BannerImages.Add(bannerImage);
        context.BannerMobileImages.Add(bannerMobileImage);
        return true;
    }
    
    private static async Task<bool> SeedEventsAsync(this PostgresqlDbContext context)
    {
        if (await context.Events.AnyAsync()) return false;

        var eventId = IdOf<Event>.New();
        var eventImage = EventImage.Create(
            id: IdOf<EventImage>.New(),
            url: "https://via.placeholder.com/800x600.png?text=Test+Event+Image",
            contentType: "image/png",
            eventId: eventId);

        var @event = Event.Create(
            id: eventId,
            image: eventImage,
            title: "Первое тестовое событие",
            shortDescription: "Краткое описание тестового события для проверки карточек на главной странице.",
            fullText: "<p>Это подробный текст тестового события, поддерживающий HTML-разметку.</p>",
            date: DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
            time: new TimeOnly(18, 0),
            location: "Челябинск, ул. Бейвеля, 22",
            status: EventStatus.Published
        );

        context.Events.Add(@event);
        context.EventImages.Add(eventImage);
        return true;
    }
    
    private static async Task<bool> SeedReviewsAsync(this PostgresqlDbContext context)
    {
        if (await context.Reviews.AnyAsync()) return false;

        var review = Review.Create(
            reviewerName: "Екатерина",
            reviewerAge: 34,
            reviewerStatus: ReviewerStatus.Patient,
            text: "Очень благодарна психологам фонда за чуткость и поддержку в сложный период жизни. Ваши консультации помогли мне вернуть уверенность.",
            status: ReviewStatus.Published
        );

        context.Reviews.Add(review);
        return true;
    }
    
    private static async Task<bool> SeedConsultationRequestsAsync(this PostgresqlDbContext context)
    {
        if (await context.ConsultationRequests.AnyAsync()) return false;

        var request = ConsultationRequest.Create(
            id: IdOf<ConsultationRequest>.New(),
            patientName: "Дмитрий Петров",
            patientAge: 42,
            patientPhoneNumber: "+79991112233",
            patientEmail: "dmitry@yandex.ru",
            communicationMethods: CommunicationMethod.CallAPhone | CommunicationMethod.Telegram
        );

        context.ConsultationRequests.Add(request);
        return true;
    }
    
    private static async Task<bool> SeedSupportGroupRequestsAsync(this PostgresqlDbContext context)
    {
        if (await context.SupportGroupRequests.AnyAsync()) return false;

        var request = SupportGroupRequest.Create(
            id: IdOf<SupportGroupRequest>.New(),
            applicantName: "Ольга Семенова",
            applicantAge: 29,
            applicantPhoneNumber: "+79994445566",
            applicantEmail: "olga@mail.ru",
            communicationMethods: CommunicationMethod.WhatsApp | CommunicationMethod.Email
        );

        context.SupportGroupRequests.Add(request);
        return true;
    }
    
    private static async Task<bool> SeedVolunteerRequestsAsync(this PostgresqlDbContext context)
    {
        if (await context.VolunteerRequests.AnyAsync()) return false;

        var request = VolunteerRequest.Create(
            id: IdOf<VolunteerRequest>.New(),
            applicantName: "Сергей Смирнов",
            applicantAge: 21,
            applicantPhoneNumber: "+79997778899",
            applicantEmail: "sergey@gmail.com",
            communicationMethods: CommunicationMethod.Telegram
        );

        context.VolunteerRequests.Add(request);
        return true;
    }
}