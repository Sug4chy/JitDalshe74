using System.Text.Json;
using JitDalshe.Ui.Admin.Api;
using Refit;

namespace JitDalshe.Ui.Admin.Extensions;

public static class ApiExceptionExtensions
{
    public static ApiError DeserializeError(this ApiException exception)
        => JsonSerializer.Deserialize<ApiError>(exception.Content ?? "{}");

    public static ApiValidationError DeserializeValidationError(this ApiException exception)
        => JsonSerializer.Deserialize<ApiValidationError>(exception.Content ?? "{}");
}