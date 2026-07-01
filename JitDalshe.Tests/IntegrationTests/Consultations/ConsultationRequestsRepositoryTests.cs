using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Tests.IntegrationTests.Consultations;

public sealed class ConsultationRequestsRepositoryTests
{
    private DbContextOptions<PostgresqlDbContext> CreateNewDatabaseOptions()
    {
        return new DbContextOptionsBuilder<PostgresqlDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task add_should_save_request_to_database_correctly()
    {
        // Arrange\
        var options = CreateNewDatabaseOptions();
        await using var context = new PostgresqlDbContext(options);
        var repository = new ConsultationRequestsRepository(context);
        var requestId = IdOf<ConsultationRequest>.New();
        var request = ConsultationRequest.Create(
            id: requestId,
            patientName: "Андрей Андреев",
            patientAge: 35,
            patientPhoneNumber: "+79991112233",
            patientEmail: "andrey@test.ru",
            communicationMethods: CommunicationMethod.CallAPhone);

        await repository.AddAsync(request, CancellationToken.None);

        await using var verifyContext = new PostgresqlDbContext(options);
        var savedRequest = await verifyContext.ConsultationRequests.FirstOrDefaultAsync(x => x.Id == requestId);
        Assert.NotNull(savedRequest);
        Assert.Equal("Андрей Андреев", savedRequest.PatientName);
        Assert.Equal(35, savedRequest.PatientAge);
        Assert.Equal(RequestStatus.New, savedRequest.ConsultationRequestStatus);
    }

    [Fact]
    public async Task count_should_return_correct_number_of_filtered_requests()
    {
        // Arrange
        var options = CreateNewDatabaseOptions();
        await using var context = new PostgresqlDbContext(options);
        var repository = new ConsultationRequestsRepository(context);
        var request1 = ConsultationRequest.Create(
            id: IdOf<ConsultationRequest>.New(),
            patientName: "Пациент 1",
            patientAge: 25,
            patientPhoneNumber: "+79991112233",
            patientEmail: "1@test.ru",
            communicationMethods: CommunicationMethod.CallAPhone);
            
        var request2 = ConsultationRequest.Create(
            id: IdOf<ConsultationRequest>.New(),
            patientName: "Пациент 2",
            patientAge: 40,
            patientPhoneNumber: "+79994445566",
            patientEmail: "2@test.ru",
            communicationMethods: CommunicationMethod.CallAPhone);
        
        request2.ChangeStatus(RequestStatus.Completed);
        
        await repository.AddAsync(request1, CancellationToken.None);
        await repository.AddAsync(request2, CancellationToken.None);

        // Act
        var countNew = await repository.CountAsync(x => x.ConsultationRequestStatus == RequestStatus.New, CancellationToken.None);
        var countCompleted = await repository.CountAsync(x => x.ConsultationRequestStatus == RequestStatus.Completed, CancellationToken.None);

        // Assert
        Assert.Equal(1, countNew);
        Assert.Equal(1, countCompleted);
    }
}