using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.UseCases.Banners.EditBanner;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;
using NSubstitute;

namespace JitDalshe.Tests.UnitTests.Banners;

public sealed class EditBannerTests
{
    private readonly IBannersRepository _bannersRepository;
    private readonly EditBannerUseCase _useCase;

    public EditBannerTests()
    {
        _bannersRepository = Substitute.For<IBannersRepository>();
        _useCase = new EditBannerUseCase(_bannersRepository);
    }

    [Fact]
    public async Task edit_should_return_not_found_when_banner_does_not_exist()
    {
        // Arrange
        var bannerId = IdOf<Banner>.New();
        _bannersRepository.FindByIdAsync(bannerId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Banner>.None);

        // Act
        var result = await _useCase.EditAsync(
            bannerId, "Title", "Description", BannerStatus.Published, false, null, null, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Баннер не найден", result.Error.Message);
        Assert.Equal(ErrorGroup.NotFound, result.Error.Group);
        await _bannersRepository.DidNotReceive().EditBannerAsync(Arg.Any<Banner>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task edit_should_update_all_fields_on_the_entity_and_persist_changes()
    {
        // Arrange
        var bannerId = IdOf<Banner>.New();
        var banner = Banner.Create(
            id: bannerId,
            title: "Старый заголовок",
            description: "Старое описание",
            redirectOnClickUrl: null,
            displayOrder: null,
            image: null,
            mobileImage: null,
            status: BannerStatus.NotPublished);

        _bannersRepository.FindByIdAsync(bannerId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Banner>.From(banner));

        // Act
        var result = await _useCase.EditAsync(
            bannerId: bannerId,
            title: "Новый заголовок",
            description: "Новое описание",
            status: BannerStatus.Published,
            isClickable: true,
            redirectOnClickUrl: "https://example.com",
            displayOrder: 3,
            ct: CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Новый заголовок", banner.Title);
        Assert.Equal("Новое описание", banner.Description);
        Assert.Equal(BannerStatus.Published, banner.Status);
        Assert.True(banner.IsClickable);
        Assert.Equal("https://example.com", banner.RedirectOnClickUrl);
        Assert.Equal(3, banner.DisplayOrder);
        await _bannersRepository.Received(1).EditBannerAsync(banner, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task edit_should_clear_display_order_when_not_provided()
    {
        // Arrange
        var bannerId = IdOf<Banner>.New();
        var banner = Banner.Create(
            id: bannerId,
            title: "Заголовок",
            description: null,
            redirectOnClickUrl: null,
            displayOrder: 2,
            image: null,
            mobileImage: null,
            status: BannerStatus.Published);

        _bannersRepository.FindByIdAsync(bannerId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Banner>.From(banner));

        // Act
        var result = await _useCase.EditAsync(
            bannerId: bannerId,
            title: "Заголовок",
            description: null,
            status: BannerStatus.NotPublished,
            isClickable: false,
            redirectOnClickUrl: null,
            displayOrder: null,
            ct: CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(banner.DisplayOrder);
        Assert.Equal(BannerStatus.NotPublished, banner.Status);
    }
}