using System.Net.Http.Json;
using VaughnLive.Models;

namespace VaughnLive.Services;

public class StreamingService : IStreamingService
{
    private readonly HttpClient _httpClient;

    public StreamingService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("VaughnLiveApi");
    }

    public async Task<List<Stream>> GetRecommendedStreamsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Stream>>("api/streams/recommended");
        return response ?? new List<Stream>();
    }

    public async Task<List<Stream>> GetTrendingStreamsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Stream>>("api/streams/trending");
        return response ?? new List<Stream>();
    }

    public async Task<StreamPlaybackInfo?> GetPlaybackInfoAsync(string streamId)
    {
        return await _httpClient.GetFromJsonAsync<StreamPlaybackInfo>($"api/streams/{streamId}/playback");
    }

    public async Task ReportStreamViewAsync(string streamId)
    {
        await _httpClient.PostAsync($"api/streams/{streamId}/view", null);
    }
}
