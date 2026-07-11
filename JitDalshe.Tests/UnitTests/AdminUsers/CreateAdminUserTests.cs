using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Abstractions.Security;
using JitDalshe.Application.Admin.UseCases.AdminUsers.CreateAdminUser;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace JitDalshe.Tests.UnitTests.AdminUsers;

public sealed class CreateAdminUserTests
{
    private readonly IAdminUsersRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly CreateAdminUserUseCase _useCase;

    public CreateAdminUserTests()
    {
        _repository = Substitute.For<IAdminUsersRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _useCase = new CreateAdminUserUseCase(_repository, _passwordHasher);
    }

    [Fact]
    public async Task create_should_succeed_when_email_is_unique()
    {
        // Arrange
        var email = "new_admin@test.ru";
        var password = "secure_password";
        var expectedHash = "password_hash_123";
        _repository.FindByEmailAsync(email, Arg.Any<CancellationToken>()).Returns(Maybe<AdminUser>.None);
        _passwordHasher.Hash(password).Returns(expectedHash);

        // Act
        var result = await _useCase.CreateAsync(email, password, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _passwordHasher.Received(1).Hash(password);
        await _repository.Received(1).AddAsync(Arg.Is<AdminUser>(u => 
            u.Email == email && 
            u.PasswordHash == expectedHash && 
            u.IsActive), 
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task create_should_fail_when_email_already_exists()
    {
        // Arrange
        var email = "existing_admin@test.ru";
        var password = "password";
        var existingUser = AdminUser.Create(IdOf<AdminUser>.New(), email, "old_hash");
        _repository.FindByEmailAsync(email, Arg.Any<CancellationToken>())
            .Returns(Maybe<AdminUser>.From(existingUser));

        // Act
        var result = await _useCase.CreateAsync(email, password, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Администратор с такой почтой уже существует", result.Error.Message);
        _passwordHasher.DidNotReceive().Hash(Arg.Any<string>());
        await _repository.DidNotReceive().AddAsync(Arg.Any<AdminUser>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task create_should_fail_when_repository_throws_exception()
    {
        // Arrange
        var email = "new_admin@test.ru";
        var password = "password";
        var exceptionMessage = "Database connection timed out";
        _repository.FindByEmailAsync(email, Arg.Any<CancellationToken>()).Returns(Maybe<AdminUser>.None);
        _passwordHasher.Hash(password).Returns("some_hash");
        _repository.AddAsync(Arg.Any<AdminUser>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _useCase.CreateAsync(email, password, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(exceptionMessage, result.Error.Message);
    }
}