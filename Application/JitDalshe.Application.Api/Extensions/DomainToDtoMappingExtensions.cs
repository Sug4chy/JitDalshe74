using JitDalshe.Application.Api.Dto;
using JitDalshe.Domain.Entities;

namespace JitDalshe.Application.Api.Extensions;

public static class DomainToDtoMappingExtensions
{
    public static NewsDto ToDto(this News news)
        => new(news.Text, news.PrimaryPhoto!.NewsPhoto!.Uri.ToString());
}