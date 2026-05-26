using System.Net.Http.Json;

namespace JitDalshe.NewsFetcher;

public sealed class VkApiClient : IDisposable
{
    private bool _disposed;
    private readonly HttpClient _httpClient;

    public VkApiClient(string apiKey)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.vk.com/method/");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }

    public async Task<VkWallGetResponse?> WallGetAsync(
        string domain,
        int offset,
        int count = 100,
        string filter = "all",
        int extended = 0,
        string? fields = null,
        CancellationToken ct = default)
    {
        var wrapper = await _httpClient.GetFromJsonAsync<VkResponseWrapper>(
            $"wall.get?domain={domain}&offset={offset}&count={count}&filter={filter}&extended={extended}&fields={fields}&v=5.131",
            ct
        );

        return wrapper?.Response;
    }
    
    private sealed class VkResponseWrapper
    {
        public VkWallGetResponse? Response { get; set; }
    }

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