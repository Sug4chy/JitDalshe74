using CSharpFunctionalExtensions;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.EditNews;

public interface IEditNewsUseCase
{
    Task<Result<NewsDto, Error>> EditAsync(
        IdOf<News>  newsId,
        string text,
        IdOf<NewsPhoto> primaryPhotoId,
        CancellationToken ct = default);
}