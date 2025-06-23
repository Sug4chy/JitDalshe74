using System.Net;
using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.Banners;
using JitDalshe.Ui.Admin.Api.Banners.Requests;
using JitDalshe.Ui.Admin.Api.Errors;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Services.Shared;
using Refit;

namespace JitDalshe.Ui.Admin.Services.BannerService;

public sealed class BannerService : IBannerService
{
    private readonly Runner _runner;
    private readonly IBannersApiClient _bannersApi;
    private readonly IToastService _toastService;

    public BannerService(Runner runner, IBannersApiClient bannersApi, IToastService toastService)
    {
        _runner = runner;
        _bannersApi = bannersApi;
        _toastService = toastService;
    }

    private void HandleError(
        HttpStatusCode statusCode,
        ApiException apiError)
    {
        ApiError error;

        switch ((int)statusCode / 100)
        {
            case 4 when statusCode is HttpStatusCode.BadRequest:
            {
                var validationError = apiError.DeserializeValidationError();
                _toastService.ShowWarning(validationError.Errors.First().Value.First());
                return;
            }
            case 4:
                error = apiError.DeserializeError();
                _toastService.ShowWarning(error.Message);
                return;
            case 5:
                error = apiError.DeserializeError();
                _toastService.ShowError(error.Message);
                return;
        }
    }

    public Task<PreviewBanner[]> FindPreviewBannersAsync()
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            var response = await _bannersApi.GetPreviewBannersAsync();

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

    public Task<Banner[]> FindAllAsync()
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            var response = await _bannersApi.ListBannersAsync();

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

    public Task<bool> CreateBannerAsync(CreateBannerRequest request)
        => _runner.RunShowingToastOnExceptionAsync(async () =>
        {
            var response = await _bannersApi.CreateBannerAsync(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    return true;
                default:
                    HandleError(
                        statusCode: response.StatusCode,
                        apiError: response.Error!);
                    return false;
            }
        });
}