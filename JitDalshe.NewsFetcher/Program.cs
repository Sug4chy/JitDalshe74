using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Context.Options;
using JitDalshe.Infrastructure.Persistence.Repositories;
using JitDalshe.NewsFetcher;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using var cts = new CancellationTokenSource();
var ct = cts.Token;

List<string> sizeTypesSorted = ["s", "m", "x", "o", "p", "q", "r", "y", "z", "w"];

string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Development;

var configuration =  new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddJsonFile($"appsettings.{envName}.json", optional: true)
    .Build();

string apiAccessToken = configuration["VK_API_ACCESS_TOKEN"]
    ?? throw new InvalidOperationException("VK_API_ACCESS_TOKEN env is missed");

string conString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string is missed");

await using var dbContext = new PostgresqlDbContext(
    DbContextOptionsFactory.CreateOptions<PostgresqlDbContext>(conString));

if (await dbContext.News.AnyAsync(ct))
{
    return;
}

var existingIds = await dbContext.News.Select(x => x.ExtId).ToListAsync(ct);
var processedIds = new HashSet<long>(existingIds);

using var client = new VkApiClient(apiAccessToken);
var newsRepository = new NewsRepository(dbContext);

int currentOffset = 0;

while (currentOffset % 100 == 0)
{
    var response = await client.WallGetAsync(domain: "zhitdalshe74", offset: currentOffset, count: 10, ct: ct);
    if (response is null)
    {
        Console.WriteLine($"Response on offset {currentOffset} is null");
        return;
    }

    currentOffset += response.Count;
    
    if (response.Count == 0)
    {
        break;
    }
    
    foreach (var wallPost in response.Items)
    {
        if (!processedIds.Add(wallPost.Id))
        {
            continue;
        }
        
        var photoAttachments = wallPost.Attachments.Where(x => x.Type is "photo");
        var newsImages = photoAttachments
            .Select(x => NewsImage.Create(
                id: IdOf<NewsImage>.New(),
                extId: x.Photo!.Id,
                url: new Uri(x.Photo.Sizes.MaxBy(s => sizeTypesSorted.IndexOf(s.Type))!.Url))
            )
            .ToList();
        
        var news = News.Create(
            id: IdOf<News>.New(),
            extId: wallPost.Id,
            text: wallPost.Text,
            publicationDate: DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(wallPost.Date).Date),
            postUrl: $"https://vk.com/wall-177662413_{wallPost.Id}",
            isDisplaying: false,
            images: newsImages);

        if (news.Images.Count is not 0)
        {
            news.PrimaryImage = NewsPrimaryImage.Create(
                newsId: news.Id,
                newsImageId: news.Images.First().Id);
        }
        
        await newsRepository.AddAsync(news, ct);
    }

    Console.WriteLine($"Fetched {response.Count} items");
}

Console.WriteLine("Fetching finished");