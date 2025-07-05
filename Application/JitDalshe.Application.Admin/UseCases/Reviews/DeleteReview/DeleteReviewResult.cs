using OneOf;
using static JitDalshe.Application.Admin.UseCases.Reviews.DeleteReview.DeleteReviewResult;

namespace JitDalshe.Application.Admin.UseCases.Reviews.DeleteReview;

[GenerateOneOf]
public sealed partial class DeleteReviewResult : OneOfBase<Deleted, NotFound, Error>
{
    public sealed class Deleted;

    public static readonly Deleted Success = new();

    public sealed class NotFound;

    public static readonly NotFound ReviewNotFound = new();

    public sealed class Error
    {
        public required string Message { get; init; }
    }

    public static Error Failure(string message) => new() { Message = message };
}