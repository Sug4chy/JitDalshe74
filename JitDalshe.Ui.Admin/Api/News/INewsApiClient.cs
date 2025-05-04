using JitDalshe.Ui.Admin.Api.News.Requests;
using Refit;

namespace JitDalshe.Ui.Admin.Api.News;

public interface INewsApiClient
{
    [Get("")]
    Task<IApiResponse<Models.News[]>> ListNewsAsync();

    [Put("/{id}")]
    Task<IApiResponse<Models.News>> EditNewsAsync(Guid id, [Body] EditNewsRequest request);

    [Delete("/{id}")]
    Task<IApiResponse> DeleteNewsByIdAsync(Guid id);
}