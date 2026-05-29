using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Reviews.ChangeStatus;

public interface IChangeReviewStatusUseCase
{
    Task<UnitResult<Error>> ChangeStatusAsync(IdOf<Review> reviewId, ReviewStatus newStatus, CancellationToken ct = default);

}