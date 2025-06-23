using CSharpFunctionalExtensions;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.News.EditNews;

public interface IEditNewsUseCase
{
    Task<Result<NewsDto, Error>> EditAsync(
        IdOf<Domain.Entities.News.News>  newsId,
        string text,
        IdOf<NewsImage> primaryPhotoId,
        bool isDisplaying,
        CancellationToken ct = default);
}