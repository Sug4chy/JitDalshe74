using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.UseCases.Banners.DeleteBanner;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;
using NSubstitute;


namespace JitDalshe.Tests.UnitTests.Banners;

public sealed class DeleteBannerTests
{
     private readonly IBannersRepository _bannersRepository;
     private readonly IImageStorage _imageStorage;
     private readonly DeleteBannerUseCase _deleteBannerUseCase;
     
     public DeleteBannerTests()
     {
          _bannersRepository = Substitute.For<IBannersRepository>();
          _imageStorage = Substitute.For<IImageStorage>();
          _deleteBannerUseCase = new DeleteBannerUseCase(_bannersRepository, _imageStorage);
     }
     
     [Fact]
     public async Task delete_should_return_not_found_when_banner_does_not_exist()
     {
          // Arrange
          IdOf<Banner> bannerId = IdOf<Banner>.New();
          _bannersRepository.FindByIdAsync(Arg.Any<IdOf<Banner>>(), Arg.Any<CancellationToken>())
               .Returns(Maybe<Banner>.None);

          var result = await _deleteBannerUseCase.DeleteAsync(bannerId, CancellationToken.None);

          Assert.True(result.IsFailure);
          Assert.Equal("Баннер не найден", result.Error.Message);
          Assert.Equal(ErrorGroup.NotFound, result.Error.Group);
     }

     [Fact]
     public async Task delete_should_remove_banner_and_image_when_banner_exists()
     {
          // Arrange
          IdOf<Banner> bannerId = IdOf<Banner>.New();
          IdOf<BannerImage> bannerImageId = IdOf<BannerImage>.New();
          BannerImage bannerImage = BannerImage.Create(bannerImageId, "", "", bannerId);
          Banner banner = Banner.Create(bannerId, null, null, null, null,  bannerImage, null);
          _bannersRepository.FindByIdAsync(bannerId, Arg.Any<CancellationToken>())
               .Returns(Maybe<Banner>.From(banner));
          
          // Act
          var result =  await _deleteBannerUseCase.DeleteAsync(bannerId, CancellationToken.None);

          // Assert
          Assert.True(result.IsSuccess);
          await _bannersRepository.Received(1).RemoveBannerAsync(banner, Arg.Any<CancellationToken>());
          await _imageStorage.Received(1).RemoveImageAsync(bannerImageId, Arg.Any<CancellationToken>());
     }
}