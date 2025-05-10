using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.Extensions;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.EditNews;

[UseCase]
internal sealed class EditNewsUseCase : IEditNewsUseCase
{
    private readonly INewsRepository _newsRepository;

    public EditNewsUseCase(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<Result<NewsDto, Error>> EditAsync(
        IdOf<News> newsId, 
        string text, 
        IdOf<NewsImage> primaryPhotoId, 
        CancellationToken ct = default)
    {
        try
        {
            var maybeNews = await _newsRepository.GetNewsByIdAsync(newsId, ct);
            if (maybeNews.HasNoValue)
            {
                return Result.Failure<NewsDto, Error>(
                    Error.Of($"News with id: {newsId} was not found.", ErrorGroup.NotFound));
            }

            var news = maybeNews.Value;

            if (news.Images.Count != 0)
            {
                var maybeNewPrimaryPhoto = news.Images.TryFirst(x => x.Id == primaryPhotoId);
                if (maybeNewPrimaryPhoto.HasNoValue)
                {
                    return Result.Failure<NewsDto, Error>(
                        Error.Of($"Photo with id: {primaryPhotoId} doesn't exist", ErrorGroup.NotFound));
                }

                news.PrimaryImage = new NewsPrimaryImage
                {
                    NewsId = news.Id,
                    News = news,
                    NewsImage = maybeNewPrimaryPhoto.Value,
                    NewsImageId = primaryPhotoId
                };
            }

            news.Text = text;
            await _newsRepository.EditAsync(news, ct);

            return Result.Success<NewsDto, Error>(news.ToDto());
        }
        catch (Exception e)
        {
            return Result.Failure<NewsDto, Error>(Error.Of(e.Message));
        }
    }
}