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

    public async Task<List<LiveStream>> GetRecommendedStreamsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<LiveStream>>("api/streams/recommended");
        return response ?? new List<LiveStream>();
    }

    public async Task<List<LiveStream>> GetTrendingStreamsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<LiveStream>>("api/streams/trending");
        return response ?? new List<LiveStream>();
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
