using JitDalshe.Application.Site.Dto;
using OneOf;

namespace JitDalshe.Application.Site.UseCases.Reviews.ListReviews;

[GenerateOneOf]
public sealed partial class ListReviewsResult : OneOfBase<ListReviewsResult.Found,  ListReviewsResult.Error>
{
    public sealed class Found
    {
        public required ReviewDto[] Reviews { get; init; }
    }

    public static Found Success(ReviewDto[] reviews) => new() { Reviews = reviews };

    public sealed class Error
    {
        public required Errors.Error InternalError { get; init; }
    }

    public static Error Failure(Errors.Error error) => new() { InternalError = error };
}