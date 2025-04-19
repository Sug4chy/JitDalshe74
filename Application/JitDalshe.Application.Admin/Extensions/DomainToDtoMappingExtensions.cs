using JitDalshe.Application.Admin.Dto;
using JitDalshe.Domain.Entities;

namespace JitDalshe.Application.Admin.Extensions;

public static class DomainToDtoMappingExtensions
{
    public static NewsDto ToDto(this News news)
        => new(
            Id: news.Id,
            Text: news.Text,
            Photos: news.Photos
                .Select(x => x.ToDto(news.PrimaryPhoto is not null && x.Id == news.PrimaryPhoto.NewsPhotoId))
                .ToArray());

    public static NewsPhotoDto ToDto(this NewsPhoto newsPhoto, bool isPrimary)
        => new(
            Id: newsPhoto.Id,
            Uri: newsPhoto.Uri.ToString(),
            IsPrimary: isPrimary);
}