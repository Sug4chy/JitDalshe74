using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Tests.IntegrationTests.Reviews;

public sealed class ReviewsRepositoryTests : IntegrationTestBase
{
    [Fact]
    public async Task add_should_save_review_to_database_correctly()
    {
        // Arrange
        var options = CreateNewDatabaseOptions();
        await using var context = new PostgresqlDbContext(options);
        var repository = new ReviewsRepository(context);
        var review = Review.Create(
            reviewerName: "Екатерина",
            reviewerAge: 28,
            reviewerStatus: ReviewerStatus.Patient,
            text: "Большое спасибо специалистам фонда за чуткое отношение.",
            status: ReviewStatus.New);
        
        // Act
        await repository.AddAsync(review, CancellationToken.None);
        
        // Assert
        await using var verifyContext = new PostgresqlDbContext(options);
        var savedReview = await verifyContext.Reviews.FirstOrDefaultAsync(x => x.Id == review.Id);
        Assert.NotNull(savedReview);
        Assert.Equal("Екатерина", savedReview.ReviewerName);
        Assert.Equal(28, savedReview.ReviewerAge);
        Assert.Equal(ReviewerStatus.Patient, savedReview.ReviewerStatus);
        Assert.Equal(ReviewStatus.New, savedReview.Status);
    }

    [Fact]
    public async Task count_should_return_correct_number_of_reviews_by_status()
    {
        // Arrange
        var options = CreateNewDatabaseOptions();
        await using var context = new PostgresqlDbContext(options);
        var repository = new ReviewsRepository(context);
        var review1 = Review.Create("Ольга", 34, ReviewerStatus.PatientRelative, "Текст 1", ReviewStatus.Published);
        var review2 = Review.Create("Дмитрий", 45, ReviewerStatus.Other, "Текст 2", ReviewStatus.Published);
        var review3 = Review.Create("Анна", 22, ReviewerStatus.Patient, "Текст 3", ReviewStatus.New);

        await repository.AddAsync(review1, CancellationToken.None);
        await repository.AddAsync(review2, CancellationToken.None);
        await repository.AddAsync(review3, CancellationToken.None);

        // Act
        var countPublished = await repository.CountAsync(x => x.Status == ReviewStatus.Published, CancellationToken.None);
        var countNew = await repository.CountAsync(x => x.Status == ReviewStatus.New, CancellationToken.None);

        // Assert
        Assert.Equal(2, countPublished);
        Assert.Equal(1, countNew);
    }
}