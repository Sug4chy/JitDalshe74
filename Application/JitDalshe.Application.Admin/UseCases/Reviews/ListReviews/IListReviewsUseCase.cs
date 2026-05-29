using CSharpFunctionalExtensions;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Entities.Reviews;

namespace JitDalshe.Application.Admin.UseCases.Reviews.ListReviews;

public interface IListReviewsUseCase
{
    Task<Result<PagedResult<ReviewDto>, Error>> ListAsync(
        int pageNumber,
        int pageSize,
        ReviewStatus? status = null,
        CancellationToken ct = default);
}