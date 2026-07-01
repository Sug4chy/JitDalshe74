using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.UseCases.Banners.CreateBanner;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;
using NSubstitute;

namespace JitDalshe.Tests.UnitTests.Banners;

public sealed class CreateBannerTests
{
    private readonly IBannersRepository _bannersRepository;
    private readonly IImageStorage _imageStorage;
    private readonly CreateBannerUseCase _createBannerUseCase;
     
    public CreateBannerTests()
    {
        _bannersRepository = Substitute.For<IBannersRepository>();
        _imageStorage = Substitute.For<IImageStorage>();
        _createBannerUseCase = new CreateBannerUseCase(_bannersRepository, _imageStorage, "test/[entity]/[id]");
    }

    [Theory] [InlineData(null, false, null, null, true)] [InlineData(null, true, "https://google.com/", 1, true)]
    public async Task create_new_banner_should_be_succesfull_or_fail_based_on_parameters(string? title, bool isClickable, string? redirectOnClickUrl, int? displayOrder, bool shouldBeSuccessful)
    {
        // Arrange
        var base64Image = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==";
        var imageId = IdOf<BannerImage>.New();
        var mobileImageId = IdOf<BannerMobileImage>.New();
        _imageStorage.SaveImageAsync<BannerImage>(Arg.Any<byte[]>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(imageId);
        _imageStorage.SaveImageAsync<BannerMobileImage>(Arg.Any<byte[]>(), Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(mobileImageId);
        
        // Act
        var result = await _createBannerUseCase.CreateAsync(title, null, base64Image, base64Image, isClickable, redirectOnClickUrl, displayOrder, CancellationToken.None);
        
        // Assert
        Assert.Equal(shouldBeSuccessful, result.IsSuccess);
        if (shouldBeSuccessful)
            await _bannersRepository.Received(1).AddAsync(Arg.Any<Banner>(), Arg.Any<CancellationToken>());
        else
            await _bannersRepository.DidNotReceive().AddAsync(Arg.Any<Banner>(), Arg.Any<CancellationToken>());
    }
}