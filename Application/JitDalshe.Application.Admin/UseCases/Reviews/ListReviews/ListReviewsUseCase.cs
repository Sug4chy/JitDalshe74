using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.Extensions;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Enums;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Entities.Reviews;

namespace JitDalshe.Application.Admin.UseCases.Reviews.ListReviews;

[UseCase]
internal sealed class ListReviewsUseCase(IReviewsRepository reviews) : IListReviewsUseCase
{
    public async Task<Result<PagedResult<ReviewDto>, Error>> ListAsync(
        int pageNumber,
        int pageSize,
        ReviewStatus? status = null,
        CancellationToken ct = default)
    {
        try
        {
            Expression<Func<Review, bool>>? filteringExpression = status.HasValue
                ? x => x.Status == status.Value
                : null;

            var totalCount = await reviews.CountAsync(filteringExpression, ct);

            var reviews1 = await reviews.FindAllAsync(
                pageNumber: pageNumber,
                pageSize: pageSize,
                filteringExpression: filteringExpression,
                orderByExpression: x => x.CreatedAt,
                sortingOrder: SortingOrder.Descending,
                ct: ct);

            var dtos = reviews1.Select(x => x.ToDto()).ToArray();

            return Result.Success<PagedResult<ReviewDto>, Error>(
                new PagedResult<ReviewDto>(
                    Items: dtos,
                    TotalCount: totalCount,
                    PageNumber: pageNumber,
                    PageSize: pageSize)
            );
        }
        catch (Exception e)
        {
            return Result.Failure<PagedResult<ReviewDto>, Error>(Error.Of(e.Message));
        }
    }
}