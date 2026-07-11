using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.UseCases.AdminUsers.ToggleAdminUserStatus;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace JitDalshe.Tests.UnitTests.AdminUsers;

public sealed class ToggleAdminUserStatusTests
{
    private readonly IAdminUsersRepository _repository;
    private readonly ToggleAdminUserStatusUseCase _useCase;

    public ToggleAdminUserStatusTests()
    {
        _repository = Substitute.For<IAdminUsersRepository>();
        _useCase = new ToggleAdminUserStatusUseCase(_repository);
    }

    [Fact]
    public async Task deactivate_should_succeed_for_standard_admin()
    {
        // Arrange
        var id = IdOf<AdminUser>.New();
        var user = AdminUser.Create(id, "standard@test.ru", "hash", UserRole.Standard);
        _repository.FindByIdAsync(id, Arg.Any<CancellationToken>()).Returns(Maybe<AdminUser>.From(user));

        // Act
        var result = await _useCase.ChangeStatusAsync(id, isActive: false, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(user.IsActive);
        await _repository.Received(1).EditAsync(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task deactivate_should_fail_for_super_admin()
    {
        // Arrange
        var id = IdOf<AdminUser>.New();
        var user = AdminUser.Create(id, "super@test.ru", "hash", UserRole.SuperAdmin);
        _repository.FindByIdAsync(id, Arg.Any<CancellationToken>()).Returns(Maybe<AdminUser>.From(user));

        // Act
        var result = await _useCase.ChangeStatusAsync(id, isActive: false, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Роль суперадмина нельзя деактивировать", result.Error.Message);
        Assert.True(user.IsActive);
        await _repository.DidNotReceive().EditAsync(Arg.Any<AdminUser>(), Arg.Any<CancellationToken>());
    }
}