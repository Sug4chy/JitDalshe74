using JitDalshe.Application.AdminApi.Dto;
using JitDalshe.Domain.Entities;

namespace JitDalshe.Application.AdminApi.Extensions;

public static class DomainToDtoMappingExtensions
{
    public static NewsDto ToDto(this News news)
        => new(
            Id: news.Id,
            Text: news.Text,
            Photos: news.Photos
                .Select(x => x.ToDto(x.PrimaryPhoto is not null))
                .ToArray());

    public static NewsPhotoDto ToDto(this NewsPhoto newsPhoto, bool isPrimary)
        => new(
            Id: newsPhoto.Id,
            Uri: newsPhoto.Uri.ToString(),
            IsPrimary: isPrimary);
}