using JitDalshe.Ui.Admin.Api.News.Requests;
using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Services.NewsService;

public interface INewsService
{
    Task<News[]> FindAllAsync();
    Task EditAsync(Guid id, EditNewsRequest request, Func<Task>? onSuccess = null);
    Task DeleteAsync(Guid id, Func<Task>? onSuccess = null);
}