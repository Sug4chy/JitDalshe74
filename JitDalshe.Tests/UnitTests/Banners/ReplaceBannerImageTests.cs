using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.UseCases.Banners.ReplaceBannerImage.Desktop;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Exceptions;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace JitDalshe.Tests.UnitTests.Banners;

public sealed class ReplaceBannerImageTests
{
    private const string Base64Image =
        "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==";

    private readonly IBannersRepository _bannersRepository;
    private readonly IImageStorage _imageStorage;
    private readonly ReplaceBannerImageUseCase _useCase;

    public ReplaceBannerImageTests()
    {
        _bannersRepository = Substitute.For<IBannersRepository>();
        _imageStorage = Substitute.For<IImageStorage>();
        _useCase = new ReplaceBannerImageUseCase(_bannersRepository, _imageStorage);
    }

    [Fact]
    public async Task replace_should_return_not_found_when_banner_does_not_exist()
    {
        // Arrange
        var bannerId = IdOf<Banner>.New();
        _bannersRepository.FindByIdAsync(bannerId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Banner>.None);

        // Act
        var result = await _useCase.ReplaceAsync(bannerId, Base64Image, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Баннер не найден", result.Error.Message);
        Assert.Equal(ErrorGroup.NotFound, result.Error.Group);
        await _imageStorage.DidNotReceive().RemoveImageAsync(Arg.Any<IdOf<BannerImage>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task replace_should_remove_old_image_and_persist_new_one_when_banner_exists()
    {
        // Arrange
        var bannerId = IdOf<Banner>.New();
        var oldImageId = IdOf<BannerImage>.New();
        var oldImage = BannerImage.Create(oldImageId, "http://old/image.png", "image/png", bannerId);
        var banner = Banner.Create(bannerId, "Заголовок", null, null, null, oldImage, null, BannerStatus.Published);
        var newImageId = IdOf<BannerImage>.New();

        _bannersRepository.FindByIdAsync(bannerId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Banner>.From(banner));
        _imageStorage.SaveImageAsync<BannerImage>(Arg.Any<byte[]>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(newImageId);

        // Act
        var result = await _useCase.ReplaceAsync(bannerId, Base64Image, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        await _imageStorage.Received(1).RemoveImageAsync(oldImageId, Arg.Any<CancellationToken>());
        await _bannersRepository.Received(1).ReplaceBannerImageAsync(
            banner,
            Arg.Is<BannerImage>(img => img.Id == newImageId && img.Url == oldImage.Url && img.BannerId == bannerId),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task replace_should_return_not_found_when_old_image_is_missing_from_storage()
    {
        // Arrange
        var bannerId = IdOf<Banner>.New();
        var oldImageId = IdOf<BannerImage>.New();
        var oldImage = BannerImage.Create(oldImageId, "http://old/image.png", "image/png", bannerId);
        var banner = Banner.Create(bannerId, "Заголовок", null, null, null, oldImage, null, BannerStatus.Published);

        _bannersRepository.FindByIdAsync(bannerId, Arg.Any<CancellationToken>())
            .Returns(Maybe<Banner>.From(banner));
        _imageStorage.RemoveImageAsync(oldImageId, Arg.Any<CancellationToken>())
            .ThrowsAsync(new ImageNotFoundException());

        // Act
        var result = await _useCase.ReplaceAsync(bannerId, Base64Image, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Изображение не найдено", result.Error.Message);
        Assert.Equal(ErrorGroup.NotFound, result.Error.Group);
        await _imageStorage.DidNotReceive().SaveImageAsync<BannerImage>(Arg.Any<byte[]>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
        await _bannersRepository.DidNotReceive().ReplaceBannerImageAsync(Arg.Any<Banner>(), Arg.Any<BannerImage>(), Arg.Any<CancellationToken>());
    }
}