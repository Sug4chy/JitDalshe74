using JitDalshe.Application.Admin.Dto;
using OneOf;

namespace JitDalshe.Application.Admin.UseCases.Reviews.ListUnmoderatedReviews;

[GenerateOneOf]
public sealed partial class ListUnmoderatedReviewsResult 
    : OneOfBase<ListUnmoderatedReviewsResult.Found,  ListUnmoderatedReviewsResult.Error>
{
    public sealed class Found
    {
        public required UnmoderatedReviewDto[] Reviews { get; init; }
    }

    public static Found Success(UnmoderatedReviewDto[] reviews) => new() { Reviews = reviews };

    public sealed class Error
    {
        public required string Message { get; init; }
    }

    public static Error Failure(string message) =>  new() { Message = message };
}