using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.UseCases.Consultations.UpdateComment;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace JitDalshe.Tests.UnitTests.Consultations;

public sealed class UpdateConsultationRequestCommentTests
{
    private readonly IConsultationRequestsRepository _requestsRepository;
    private readonly UpdateConsultationRequestCommentUseCase _useCase;

    public UpdateConsultationRequestCommentTests()
    {
        _requestsRepository = Substitute.For<IConsultationRequestsRepository>();
        _useCase = new UpdateConsultationRequestCommentUseCase(_requestsRepository);
    }

    [Fact]
    public async Task update_comment_should_return_not_found_when_request_does_not_exist()
    {
        // Arrange
        var requestId = IdOf<ConsultationRequest>.New();
        _requestsRepository.FindByIdAsync(requestId, Arg.Any<CancellationToken>())
            .Returns(Maybe<ConsultationRequest>.None);

        // Act
        var result = await _useCase.UpdateAsync(requestId, "Комментарий", CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Заявка не найдена", result.Error.Message);
        Assert.Equal(ErrorGroup.NotFound, result.Error.Group);
        await _requestsRepository.DidNotReceive().EditAsync(Arg.Any<ConsultationRequest>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task update_comment_should_set_new_comment_on_entity_and_persist_when_request_exists()
    {
        // Arrange
        var request = ConsultationRequest.Create(
            IdOf<ConsultationRequest>.New(), "Иван Иванов", 40, "+79990001122", "ivan@test.ru", CommunicationMethod.Email);
        _requestsRepository.FindByIdAsync(request.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<ConsultationRequest>.From(request));
        
        // Act
        var result = await _useCase.UpdateAsync(request.Id, "Перезвонить завтра в 10:00", CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Перезвонить завтра в 10:00", request.Comment);
        await _requestsRepository.Received(1).EditAsync(request, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task update_comment_should_allow_clearing_existing_comment_with_null()
    {
        // Arrange
        var request = ConsultationRequest.Create(
            IdOf<ConsultationRequest>.New(), "Пётр Петров", 35, "+79990001133", null, CommunicationMethod.CallAPhone);
        request.UpdateComment("Старый комментарий");
        _requestsRepository.FindByIdAsync(request.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<ConsultationRequest>.From(request));

        // Act
        var result = await _useCase.UpdateAsync(request.Id, null, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(request.Comment);
    }
}