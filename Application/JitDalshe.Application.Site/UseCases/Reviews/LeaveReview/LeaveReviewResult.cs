using OneOf;

namespace JitDalshe.Application.Site.UseCases.Reviews.LeaveReview;

[GenerateOneOf]
public sealed partial class LeaveReviewResult 
    : OneOfBase<LeaveReviewResult.Leaved, LeaveReviewResult.Error>
{
    public sealed class Leaved;

    public static Leaved Success() => new();

    public sealed class Error
    {
        public required string Message { get; init; }
    }

    public static Error Failed(string message) => new() { Message = message };
}