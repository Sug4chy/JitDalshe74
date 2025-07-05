using OneOf;
using static JitDalshe.Application.Admin.UseCases.Reviews.ModerateReview.ModerateReviewResult;

namespace JitDalshe.Application.Admin.UseCases.Reviews.ModerateReview;

[GenerateOneOf]
public sealed partial class ModerateReviewResult 
    : OneOfBase<
        ModeratedSuccessfully,
        ReviewNotFound,
        AlreadyModerated, 
        Error>
{
    public sealed class ModeratedSuccessfully;

    public static ModeratedSuccessfully Success() => new();

    public sealed class ReviewNotFound;

    public static ReviewNotFound NotFound() => new();

    public sealed class AlreadyModerated;

    public static AlreadyModerated ReviewAlreadyModerated() => new();

    public sealed class Error
    {
        public required string Message { get; init; }
    }

    public static Error Failure(string message) => new() { Message = message };
}