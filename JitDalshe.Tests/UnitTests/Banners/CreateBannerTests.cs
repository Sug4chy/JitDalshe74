using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.UseCases.Banners.CreateBanner;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace JitDalshe.Tests.UnitTests.Banners;

public sealed class CreateBannerTests
{
    private const string Base64Image =
        "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==";

    private readonly IBannersRepository _bannersRepository;
    private readonly IImageStorage _imageStorage;
    private readonly CreateBannerUseCase _createBannerUseCase;

    public CreateBannerTests()
    {
        _bannersRepository = Substitute.For<IBannersRepository>();
        _imageStorage = Substitute.For<IImageStorage>();
        _createBannerUseCase = new CreateBannerUseCase(_bannersRepository, _imageStorage, "test/[entity]/[id]");
    }
    
    [Theory]
    [InlineData(null, false, null, null, true)]
    [InlineData(null, true, "https://google.com/", 1, true)]
    public async Task create_new_banner_should_be_succesfull_or_fail_based_on_parameters(string? title, bool isClickable, string? redirectOnClickUrl, int? displayOrder, bool shouldBeSuccessful)
    {
        // Arrange
        var imageId = IdOf<BannerImage>.New();
        var mobileImageId = IdOf<BannerMobileImage>.New();
        _imageStorage.SaveImageAsync<BannerImage>(Arg.Any<byte[]>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(imageId);
        _imageStorage.SaveImageAsync<BannerMobileImage>(Arg.Any<byte[]>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(mobileImageId);

        // Act
        var result = await _createBannerUseCase.CreateAsync(title, null, Base64Image, Base64Image, isClickable, redirectOnClickUrl, displayOrder, CancellationToken.None);

        // Assert
        Assert.Equal(shouldBeSuccessful, result.IsSuccess);
        if (shouldBeSuccessful)
            await _bannersRepository.Received(1).AddAsync(Arg.Any<Banner>(), Arg.Any<CancellationToken>());
        else
            await _bannersRepository.DidNotReceive().AddAsync(Arg.Any<Banner>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task create_with_display_order_that_is_already_taken_should_unset_order_on_old_banner()
    {
        // Arrang
        var existingBannerId = IdOf<Banner>.New();
        var existingBanner = Banner.Create(
            id: existingBannerId,
            title: "Старый баннер",
            description: null,
            redirectOnClickUrl: null,
            displayOrder: 1,
            image: null,
            mobileImage: null,
            status: BannerStatus.Published);
        
        _bannersRepository.FindDisplayingBannersAsync(Arg.Any<CancellationToken>())
            .Returns([existingBanner]);
        _imageStorage.SaveImageAsync<BannerImage>(Arg.Any<byte[]>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(IdOf<BannerImage>.New());
        _imageStorage.SaveImageAsync<BannerMobileImage>(Arg.Any<byte[]>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(IdOf<BannerMobileImage>.New());

        // Act
        var result = await _createBannerUseCase.CreateAsync(
            title: "Новый баннер",
            description: null,
            imageBase64Url: Base64Image,
            mobileImageBase64Url: Base64Image,
            isClickable: false,
            redirectOnClickUrl: null,
            displayOrder: 1,
            ct: CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(existingBanner.DisplayOrder);
        await _bannersRepository.Received(1).EditBannerAsync(
            Arg.Is<Banner>(b => b.Id == existingBannerId && b.DisplayOrder == null),
            Arg.Any<CancellationToken>());
        await _bannersRepository.Received(1).AddAsync(
            Arg.Is<Banner>(b => b.Title == "Новый баннер" && b.DisplayOrder == 1),
            Arg.Any<CancellationToken>());
    }
}