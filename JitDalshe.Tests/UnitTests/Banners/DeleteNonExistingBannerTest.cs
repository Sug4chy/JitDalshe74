using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.UseCases.Banners.DeleteBanner;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;
using NSubstitute;


namespace JitDalshe.Tests.UnitTests.Banners;

public class DeleteNonExistingBannerTests
{
     private readonly IBannersRepository _bannersRepository;
     private readonly IImageStorage _imageStorage;
     private readonly DeleteBannerUseCase _deleteBannerUseCase;
     
     public DeleteNonExistingBannerTests()
     {
          _bannersRepository = Substitute.For<IBannersRepository>();
          _imageStorage = Substitute.For<IImageStorage>();
          _deleteBannerUseCase = new DeleteBannerUseCase(_bannersRepository, _imageStorage);
     }
     
     [Fact]
     public async Task DeleteNonExistingBanner()
     {
          // Arrange
          IdOf<Banner> bannerId = IdOf<Banner>.New();
          _bannersRepository.FindByIdAsync(Arg.Any<IdOf<Banner>>(), Arg.Any<CancellationToken>())
               .Returns(Maybe<Banner>.None);
          
          // Act
          var result = await _deleteBannerUseCase.DeleteAsync(bannerId, CancellationToken.None);
          
          // Assert
          Assert.True(result.IsFailure);
          Assert.Equal("Баннер не найден", result.Error.Message);
          Assert.Equal(ErrorGroup.NotFound, result.Error.Group);
     }
}