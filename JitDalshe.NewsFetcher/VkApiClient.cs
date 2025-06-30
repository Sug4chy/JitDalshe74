using System.Net.Http.Json;

namespace JitDalshe.NewsFetcher;

public sealed class VkApiClient : IDisposable
{
    private bool _disposed;
    private readonly HttpClient _httpClient;

    public VkApiClient(string apiKey)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.vk.com/method");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    public Task<VkWallGetResponse?> WallGetAsync(
        string domain,
        int offset,
        int count = 100,
        string filter = "all",
        int extended = 0,
        string? fields = null,
        CancellationToken ct = default)
        => _httpClient.GetFromJsonAsync<VkWallGetResponse>(
            $"/wall.get?domain={domain}&offset={offset}&count={count}&filter={filter}&extended={extended}&fields={fields}",
            ct
        );

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (!disposing)
        {
            return;
        }

        _httpClient.Dispose();
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
    }
}