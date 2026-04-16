using System.Net;
using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.Consultations;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Services.ErrorHandlers;
using JitDalshe.Ui.Admin.Services.Shared;

namespace JitDalshe.Ui.Admin.Services.ConsultationService;

public sealed class ConsultationService : IConsultationService
{
    private readonly Runner _runner;
    private readonly IConsultationsApiClient _consultationsApi;
    private readonly CommonErrorHandlers _commonErrorHandlers;

    public ConsultationService(
        Runner runner, 
        IConsultationsApiClient consultationsApi, 
        IToastService toastService, 
        CommonErrorHandlers commonErrorHandlers)
    {
        _runner = runner;
        _consultationsApi = consultationsApi;
        _commonErrorHandlers = commonErrorHandlers;
        // Настройка тостов для вывода ошибок из Runner
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }

    public Task<ConsultationRequest[]> ListAsync()
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _consultationsApi.ListAsync();

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

    public Task ToggleStatusAsync(Guid id, Func<Task>? onSuccess = null)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _consultationsApi.ChangeStatusAsync(id);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    if (onSuccess != null) await onSuccess.Invoke();
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