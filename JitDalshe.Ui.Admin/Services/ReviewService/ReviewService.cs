using System.Net;
using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.Reviews;
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

    public Task<UnmoderatedReview[]> FindAllUnmoderatedReviewsAsync()
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _reviewsApi.ListUnmoderatedReviewsAsync();

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content!;
                case HttpStatusCode.InternalServerError:
                    _errorHandlers.HandleInternalServerError(response.Error!);
                    return [];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }, defaultValue: []);

    public Task ModerateReviewAsync(Guid reviewId, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _reviewsApi.ModerateReviewAsync(reviewId);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    await (onSuccess?.Invoke() ?? Task.CompletedTask);
                    break;
                case HttpStatusCode.NotFound:
                    _errorHandlers.HandleNotFound(response.Error!);
                    break;
                case HttpStatusCode.Conflict:
                    _errorHandlers.HandleConflict(response.Error!);
                    break;
                case HttpStatusCode.InternalServerError:
                    _errorHandlers.HandleInternalServerError(response.Error!);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });

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