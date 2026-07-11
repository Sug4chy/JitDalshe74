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
    private readonly IErrorHandlers _errorHandlers;

    public NewsService(
        IToastService toastService, 
        INewsApiClient newsApi, 
        Runner runner, 
        IErrorHandlers errorHandlers)
    {
        _newsApi = newsApi;
        _runner = runner;
        _errorHandlers = errorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }

    public Task<News[]> FindAllAsync()
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _newsApi.ListNewsAsync();
            return response.Handle(_errorHandlers) ?? [];
        }, defaultValue: []);

    public Task EditAsync(Guid id, EditNewsRequest request, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _newsApi.EditNewsAsync(id, request);
            if (response.Handle(_errorHandlers) is not null)
            {
                await (onSuccess?.Invoke() ?? Task.CompletedTask);
            }
        });

    public Task DeleteAsync(Guid id, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _newsApi.DeleteNewsByIdAsync(id);
            if (response.Handle(_errorHandlers, HttpStatusCode.NoContent))
            {
                await (onSuccess?.Invoke() ?? Task.CompletedTask);
            }
        });

}