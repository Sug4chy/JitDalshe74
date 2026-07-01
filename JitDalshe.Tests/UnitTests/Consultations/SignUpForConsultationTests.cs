using JitDalshe.Application.Abstractions.Notifications;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Site.UseCases.Consultations;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Consultations;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace JitDalshe.Tests.UnitTests.Consultations;

public sealed class SignUpForConsultationTests
{
    private readonly IConsultationRequestsRepository _consultationRequestsRepository;
    private readonly IEmailNotificationsSender _emailNotificationsSender;
    private readonly ISignUpForConsultationUseCase _signUpForConsultationUseCase;
    
    public SignUpForConsultationTests()
    {
        _consultationRequestsRepository = Substitute.For<IConsultationRequestsRepository>();
        _emailNotificationsSender = Substitute.For<IEmailNotificationsSender>();
        _signUpForConsultationUseCase = new SignUpForConsultationUseCase(_consultationRequestsRepository, _emailNotificationsSender);
    }

    [Fact]
    public async Task signup_should_save_request_and_send_email_when_data_is_valid()
    {
        // Arrange
        var name = "Петр Первый";
        var age = 30;
        var phone = "+79991112233";
        var email = "ivan@test.ru";
        var method = CommunicationMethod.CallAPhone | CommunicationMethod.Email;
        
        var result = await _signUpForConsultationUseCase.SignUpAsync(name, age, phone, email, method, CancellationToken.None);
        
        Assert.True(result.IsT0);
        await _consultationRequestsRepository.Received(1).AddAsync(Arg.Is<ConsultationRequest>(r =>
                r.PatientName == name && 
                r.PatientAge == age && 
                r.PatientPhoneNumber == phone && 
                r.PatientEmail == email && 
                r.CommunicationMethods == method),
            Arg.Any<CancellationToken>());
        
        await _emailNotificationsSender.Received(1).SendAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task signup_should_fail_when_db_throws_exception()
    {
        // Arrange
        var name = "Петр Первый";
        var age = 30;
        var phone = "+79991112233";
        var email = "ivan@test.ru";
        var method = CommunicationMethod.CallAPhone;
        _consultationRequestsRepository.AddAsync(Arg.Any<ConsultationRequest>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception("Database connection failed"));
        
        // Act
        var result = await _signUpForConsultationUseCase.SignUpAsync(name, age, phone, email, method, CancellationToken.None);

        // Assert
        Assert.True(result.IsT1);
        Assert.Equal("Database connection failed", result.AsT1.Message);
        await _emailNotificationsSender.DidNotReceive().SendAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}