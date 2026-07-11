using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Abstractions.Security;
using JitDalshe.Application.Admin.UseCases.Auth.Login;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace JitDalshe.Tests.UnitTests.Auth;

public sealed class LoginTests
{
    private readonly IAdminUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly LoginUseCase _useCase;

    public LoginTests()
    {
        _usersRepository = Substitute.For<IAdminUsersRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtProvider = Substitute.For<IJwtProvider>();
        _useCase = new LoginUseCase(_usersRepository, _passwordHasher, _jwtProvider);
    }

    [Fact]
    public async Task login_should_return_token_when_credentials_are_valid()
    {
        // Arrange
        var email = "admin@test.ru";
        var password = "password123";
        var hash = "password_hash";
        var expectedToken = "jwt_test_token";
        var user = AdminUser.Create(IdOf<AdminUser>.New(), email, hash, UserRole.Standard);

        _usersRepository.FindByEmailAsync(email, Arg.Any<CancellationToken>())
            .Returns(Maybe<AdminUser>.From(user));
        _passwordHasher.Verify(password, hash).Returns(true);
        _jwtProvider.GenerateToken(user.Id, email, UserRole.Standard).Returns(expectedToken);

        // Act
        var result = await _useCase.LoginAsync(email, password, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedToken, result.Value);
    }

    [Fact]
    public async Task login_should_fail_when_password_is_incorrect()
    {
        // Arrange
        var email = "admin@test.ru";
        var password = "wrong_password";
        var hash = "password_hash";
        var user = AdminUser.Create(IdOf<AdminUser>.New(), email, hash, UserRole.Standard);

        _usersRepository.FindByEmailAsync(email, Arg.Any<CancellationToken>())
            .Returns(Maybe<AdminUser>.From(user));
        _passwordHasher.Verify(password, hash).Returns(false);

        // Act
        var result = await _useCase.LoginAsync(email, password, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Неверный email или пароль", result.Error.Message);
        Assert.Equal(ErrorGroup.Unauthorized, result.Error.Group);
    }

    [Fact]
    public async Task login_should_fail_when_user_is_inactive()
    {
        // Arrange
        var email = "inactive@test.ru";
        var password = "password123";
        var user = AdminUser.Create(IdOf<AdminUser>.New(), email, "hash", UserRole.Standard);
        user.Deactivate(); // Деактивируем пользователя

        _usersRepository.FindByEmailAsync(email, Arg.Any<CancellationToken>())
            .Returns(Maybe<AdminUser>.From(user));

        // Act
        var result = await _useCase.LoginAsync(email, password, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Неверный email или пароль", result.Error.Message);
    }
}