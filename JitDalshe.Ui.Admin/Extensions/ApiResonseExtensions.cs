using System.Net;
using JitDalshe.Ui.Admin.Services.ErrorHandlers;
using Refit;

namespace JitDalshe.Ui.Admin.Extensions;


public static class ApiResponseExtensions
{
    public static T? Handle<T>(this IApiResponse<T> response, IErrorHandlers errorHandlers)
    {
        if (response.IsSuccessStatusCode)
        {
            return response.Content;
        }

        ProcessError(response, errorHandlers);
        return default;
    }
    
    public static bool Handle(this IApiResponse response, IErrorHandlers errorHandlers, HttpStatusCode successStatusCode = HttpStatusCode.OK)
    {
        if (response.StatusCode == successStatusCode || response.IsSuccessStatusCode)
        {
            return true;
        }

        ProcessError(response, errorHandlers);
        return false;
    }
    
    private static void ProcessError(IApiResponse response, IErrorHandlers errorHandlers)
    {
        switch (response.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                errorHandlers.HandleBadRequest(response.Error!);
                break;
            case HttpStatusCode.NotFound:
                errorHandlers.HandleNotFound(response.Error!);
                break;
            case HttpStatusCode.Conflict:
                errorHandlers.HandleConflict(response.Error!);
                break;
            default:
                errorHandlers.HandleInternalServerError(response.Error!);
                break;
        }
    }
}