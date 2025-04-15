using JitDalshe.Application.Site.Dto;
using JitDalshe.Domain.Entities;

namespace JitDalshe.Application.Site.Extensions;

public static class DomainToDtoMappingExtensions
{
    public static NewsDto ToDto(this News news)
        => new(news.Text, news.PrimaryPhoto!.NewsPhoto!.Uri.ToString());
}