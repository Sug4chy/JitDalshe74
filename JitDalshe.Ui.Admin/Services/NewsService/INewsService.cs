using JitDalshe.Ui.Admin.Api.News.Requests;
using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Services.NewsService;

public interface INewsService
{
    Task<News[]> FindAllAsync();
    Task<bool> EditAsync(Guid id, EditNewsRequest request);
    Task<bool> DeleteAsync(Guid id);
}