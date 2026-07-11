using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.UseCases.Events.EditEvent;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace JitDalshe.Tests.UnitTests.Events;

public sealed class EditEventTests
{
    private readonly IEventsRepository _eventsRepository;
    private readonly EditEventUseCase _useCase;

    public EditEventTests()
    {
        _eventsRepository = Substitute.For<IEventsRepository>();
        _useCase = new EditEventUseCase(_eventsRepository);
    }

    [Fact]
    public async Task edit_should_return_not_found_when_event_does_not_exist()
    {
        // Arrange
        var eventId = IdOf<Event>.New();
        _eventsRepository.FindByIdAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Event>.None);

        // Act
        var result = await _useCase.EditAsync(
            eventId, "Заголовок", "Краткое описание", "Полный текст", null, null, null, EventStatus.Published, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Событие не найдено", result.Error.Message);
        Assert.Equal(ErrorGroup.NotFound, result.Error.Group);
        await _eventsRepository.DidNotReceive().EditAsync(Arg.Any<Event>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task edit_should_update_all_details_and_status_on_entity_when_event_exists()
    {
        // Arrange
        var eventId = IdOf<Event>.New();
        var image = EventImage.Create(IdOf<EventImage>.New(), "http://image.png", "image/png", eventId);
        var ev = Event.Create(
            id: eventId,
            image: image,
            title: "Старый заголовок",
            shortDescription: "Старое краткое описание",
            fullText: "Старый полный текст",
            date: null,
            time: null,
            location: null,
            status: EventStatus.NotPublished);

        _eventsRepository.FindByIdAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Event>.From(ev));

        var newDate = new DateOnly(2026, 12, 1);
        var newTime = new TimeOnly(18, 30);

        // Act
        var result = await _useCase.EditAsync(
            id: eventId,
            title: "Новый заголовок",
            shortDescription: "Новое краткое описание",
            fullText: "Новый полный текст",
            date: newDate,
            time: newTime,
            location: "Челябинск, ул. Бейвеля, 22",
            status: EventStatus.Published,
            ct: CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Новый заголовок", ev.Title);
        Assert.Equal("Новое краткое описание", ev.ShortDescription);
        Assert.Equal("Новый полный текст", ev.FullText);
        Assert.Equal(newDate, ev.Date);
        Assert.Equal(newTime, ev.Time);
        Assert.Equal("Челябинск, ул. Бейвеля, 22", ev.Location);
        Assert.Equal(EventStatus.Published, ev.Status);
        await _eventsRepository.Received(1).EditAsync(ev, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task edit_should_allow_unpublishing_a_previously_published_event()
    {
        // Arrange
        var eventId = IdOf<Event>.New();
        var image = EventImage.Create(IdOf<EventImage>.New(), "http://image.png", "image/png", eventId);
        var ev = Event.Create(
            eventId, image, "Заголовок", "Краткое", "Полное", null, null, null, EventStatus.Published);
        
        _eventsRepository.FindByIdAsync(eventId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Event>.From(ev));

        // Act
        var result = await _useCase.EditAsync(
            eventId, "Заголовок", "Краткое", "Полное", null, null, null, EventStatus.NotPublished, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.NotPublished, ev.Status);
    }
}