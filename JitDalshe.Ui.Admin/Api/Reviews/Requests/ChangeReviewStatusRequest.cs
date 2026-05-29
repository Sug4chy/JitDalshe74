using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Api.Reviews.Requests;

public sealed record ChangeReviewStatusRequest(ReviewStatus Status);