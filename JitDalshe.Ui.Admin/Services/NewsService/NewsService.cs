using System.Net;
using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.News;
using JitDalshe.Ui.Admin.Api.News.Requests;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Services.ErrorHandlers;
using JitDalshe.Ui.Admin.Services.Shared;

namespace JitDalshe.Ui.Admin.Services.NewsService;

public sealed class NewsService : INewsService
{
    private readonly INewsApiClient _newsApi;
    private readonly Runner _runner;
    private readonly CommonErrorHandlers _commonErrorHandlers;

    public NewsService(
        IToastService toastService, 
        INewsApiClient newsApi, 
        Runner runner, 
        CommonErrorHandlers commonErrorHandlers)
    {
        _newsApi = newsApi;
        _runner = runner;
        _commonErrorHandlers = commonErrorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }

    public Task<News[]> FindAllAsync()
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _newsApi.ListNewsAsync();

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content!;
                case HttpStatusCode.InternalServerError:
                    _commonErrorHandlers.HandleInternalServerError(response.Error!);
                    return [];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }, defaultValue: []);

    public Task EditAsync(Guid id, EditNewsRequest request, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _newsApi.EditNewsAsync(id, request);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    await (onSuccess?.Invoke() ?? Task.CompletedTask);
                    break;
                case HttpStatusCode.BadRequest:
                    _commonErrorHandlers.HandleBadRequest(response.Error!);
                    break;
                case HttpStatusCode.NotFound:
                    _commonErrorHandlers.HandleNotFound(response.Error!);
                    break;
                case HttpStatusCode.InternalServerError:
                    _commonErrorHandlers.HandleInternalServerError(response.Error!);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });

    public Task DeleteAsync(Guid id, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _newsApi.DeleteNewsByIdAsync(id);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    await (onSuccess?.Invoke() ?? Task.CompletedTask);
                    break;
                case HttpStatusCode.NotFound:
                    _commonErrorHandlers.HandleNotFound(response.Error!);
                    break;
                case HttpStatusCode.InternalServerError:
                    _commonErrorHandlers.HandleInternalServerError(response.Error!);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
}