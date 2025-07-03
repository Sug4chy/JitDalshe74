using System.Net;
using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.Banners;
using JitDalshe.Ui.Admin.Api.Banners.Requests;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Services.Shared;

namespace JitDalshe.Ui.Admin.Services.BannerService;

public sealed class BannerService : IBannerService
{
    private readonly Runner _runner;
    private readonly IBannersApiClient _bannersApi;
    private readonly CommonErrorHandlers _commonErrorHandlers;

    public BannerService(
        Runner runner, 
        IBannersApiClient bannersApi, 
        IToastService toastService, 
        CommonErrorHandlers commonErrorHandlers)
    {
        _runner = runner;
        _bannersApi = bannersApi;
        _commonErrorHandlers = commonErrorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }

    public Task<PreviewBanner[]> FindPreviewBannersAsync()
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _bannersApi.GetPreviewBannersAsync();

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

    public Task<Banner[]> FindAllAsync()
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _bannersApi.ListBannersAsync();

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

    public Task CreateBannerAsync(CreateBannerRequest request, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _bannersApi.CreateBannerAsync(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    await (onSuccess?.Invoke() ?? Task.CompletedTask);
                    break;
                case HttpStatusCode.BadRequest:
                    _commonErrorHandlers.HandleBadRequest(response.Error!);
                    break;
                case HttpStatusCode.InternalServerError:
                    _commonErrorHandlers.HandleInternalServerError(response.Error!);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });

    public Task EditBannerAsync(Guid id, EditBannerRequest request, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _bannersApi.EditBannerAsync(id, request);

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

    public Task ReplaceBannerImageAsync(Guid id, ReplaceBannerImageRequest request, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _bannersApi.ReplaceBannerImageAsync(id, request);

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

    public Task DeleteBannerAsync(Guid id, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _bannersApi.DeleteBannerAsync(id);

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