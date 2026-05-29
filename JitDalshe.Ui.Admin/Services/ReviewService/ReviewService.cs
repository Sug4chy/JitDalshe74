using System.Net;
using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.Reviews;
using JitDalshe.Ui.Admin.Api.Reviews.Requests;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Services.ErrorHandlers;
using JitDalshe.Ui.Admin.Services.Shared;

namespace JitDalshe.Ui.Admin.Services.ReviewService;

public sealed class ReviewService : IReviewService
{
    private readonly IReviewsApiClient _reviewsApi;
    private readonly Runner _runner;
    private readonly IErrorHandlers _errorHandlers;

    public ReviewService(
        IReviewsApiClient reviewsApi, 
        Runner runner, 
        IErrorHandlers errorHandlers,
        IToastService toastService)
    {
        _reviewsApi = reviewsApi;
        _runner = runner;
        _errorHandlers = errorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }

    public Task<PagedResult<Review>?> ListAsync(
        int pageNumber, 
        int pageSize, 
        ReviewStatus? status = null, 
        CancellationToken ct = default)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _reviewsApi.ListReviewsAsync(pageNumber, pageSize, status, ct);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content!;
                case HttpStatusCode.InternalServerError:
                    _errorHandlers.HandleInternalServerError(response.Error!);
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }, defaultValue: null);

    public Task<bool> ChangeStatusAsync(Guid id, ReviewStatus status, CancellationToken ct = default)
        => _runner.RunCatchingAsync(async () =>
        {
            var request = new ChangeReviewStatusRequest(status);
            var response = await _reviewsApi.ChangeReviewStatusAsync(id, request, ct);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;
                case HttpStatusCode.NotFound:
                    _errorHandlers.HandleNotFound(response.Error!);
                    return false;
                case HttpStatusCode.InternalServerError:
                    _errorHandlers.HandleInternalServerError(response.Error!);
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }, defaultValue: false);

    public Task DeleteReviewAsync(Guid reviewId, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _reviewsApi.DeleteReviewAsync(reviewId);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    await (onSuccess?.Invoke() ?? Task.CompletedTask);
                    break;
                case HttpStatusCode.NotFound:
                    _errorHandlers.HandleNotFound(response.Error!);
                    break;
                case HttpStatusCode.InternalServerError:
                    _errorHandlers.HandleInternalServerError(response.Error!);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
}