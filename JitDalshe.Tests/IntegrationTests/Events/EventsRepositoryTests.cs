using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Tests.IntegrationTests.Events;

public sealed class EventsRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task add_should_save_event_and_image_to_database_correctly()
    {
        // Arrange
        var options = CreateNewDatabaseOptions();
        await using var context = new PostgresqlDbContext(options);
        var repository = new EventsRepository(context);
        var eventId = IdOf<Event>.New();
        var imageId = IdOf<EventImage>.New();
        var eventImage = EventImage.Create(imageId, "http://test/image.png", "image/png", eventId);
        var @event = Event.Create(
            id: eventId,
            image: eventImage,
            title: "Название события",
            shortDescription: "Краткое описание",
            fullText: "<p>Полный текст</p>",
            date: new DateOnly(2026, 6, 1),
            time: new TimeOnly(12, 0),
            location: "Москва",
            status: EventStatus.Published);
        
        // Act
        await repository.AddAsync(@event, CancellationToken.None);
        
        // Assert
        await using var verifyContext = new PostgresqlDbContext(options);
        var savedEvent = await verifyContext.Events
            .Include(x => x.Image)
            .FirstOrDefaultAsync(x => x.Id == eventId);
        Assert.NotNull(savedEvent);
        Assert.Equal("Название события", savedEvent.Title);
        Assert.NotNull(savedEvent.Image);
        Assert.Equal(imageId, savedEvent.Image.Id);
        Assert.Equal("http://test/image.png", savedEvent.Image.Url);
    }

    [Fact]
    public async Task count_should_return_correct_total_number_of_events()
    {
        // Arrange
        var options = CreateNewDatabaseOptions();
        await using var context = new PostgresqlDbContext(options);
        var repository = new EventsRepository(context);
        
        var eventId1 = IdOf<Event>.New();
        var eventImage1 = EventImage.Create(IdOf<EventImage>.New(), "url1", "image/png", eventId1);
        var event1 = Event.Create(eventId1, eventImage1, "Событие 1", "Desc 1", "Full 1", null, null, null);
        var eventId2 = IdOf<Event>.New();
        var eventImage2 = EventImage.Create(IdOf<EventImage>.New(), "url2", "image/png", eventId2);
        var event2 = Event.Create(eventId2, eventImage2, "Событие 2", "Desc 2", "Full 2", null, null, null);
        await repository.AddAsync(event1, CancellationToken.None);
        await repository.AddAsync(event2, CancellationToken.None);
        
        // Act
        var count = await repository.CountAsync(null, CancellationToken.None);
        
        // Assert
        Assert.Equal(2, count);
    }
}