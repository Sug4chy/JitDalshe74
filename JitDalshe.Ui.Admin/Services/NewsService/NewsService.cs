using System.Net;
using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.Errors;
using JitDalshe.Ui.Admin.Api.News;
using JitDalshe.Ui.Admin.Api.News.Requests;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Services.Shared;

namespace JitDalshe.Ui.Admin.Services.NewsService;

public sealed class NewsService : INewsService
{
    private readonly INewsApiClient _newsApi;
    private readonly IToastService _toastService;
    private readonly Runner _runner;

    public NewsService(IToastService toastService, INewsApiClient newsApi, Runner runner)
    {
        _toastService = toastService;
        _newsApi = newsApi;
        _runner = runner;
    }

    public Task<News[]> FindAllAsync()
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            var response = await _newsApi.ListNewsAsync();

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content!;
                default:
                    var error = response.Error!.DeserializeError();
                    _toastService.ShowError(error.Message);
                    return [];
            }
        }, defaultValue: []);

    public Task<bool> EditAsync(Guid id, EditNewsRequest request)
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            var response = await _newsApi.EditNewsAsync(id, request);
            ApiError error;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;
                case HttpStatusCode.BadRequest:
                    var validationError = response.Error!.DeserializeValidationError();
                    _toastService.ShowWarning(validationError.Errors.First().Value.First());
                    return false;
                case HttpStatusCode.NotFound:
                    error = response.Error!.DeserializeError();
                    _toastService.ShowWarning(error.Message);
                    return false;
                default:
                    error = response.Error!.DeserializeError();
                    _toastService.ShowError(error.Message);
                    return false;
            }
        });

    public Task<bool> DeleteAsync(Guid id)
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            var response = await _newsApi.DeleteNewsByIdAsync(id);
            ApiError error;

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return true;
                case HttpStatusCode.NotFound:
                    error = response.Error!.DeserializeError();
                    _toastService.ShowWarning(error.Message);
                    return false;
                default:
                    error = response.Error!.DeserializeError();
                    _toastService.ShowError(error.Message);
                    return false;
            }
        });
}