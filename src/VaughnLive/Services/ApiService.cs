using System.Net.Http.Json;
using VaughnLive.Models;

namespace VaughnLive.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("VaughnLiveApi");
    }

    // Streams
    public async Task<List<Stream>> GetFeaturedStreamsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Stream>>("api/streams/featured");
        return response ?? new List<Stream>();
    }

    public async Task<List<Stream>> GetLiveStreamsAsync(string? category = null, int page = 1, int pageSize = 20)
    {
        var url = $"api/streams/live?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrEmpty(category))
        {
            url += $"&category={Uri.EscapeDataString(category)}";
        }

        var response = await _httpClient.GetFromJsonAsync<List<Stream>>(url);
        return response ?? new List<Stream>();
    }

    public async Task<Stream?> GetStreamAsync(string streamId)
    {
        return await _httpClient.GetFromJsonAsync<Stream>($"api/streams/{streamId}");
    }

    public async Task<List<Stream>> SearchStreamsAsync(string query, int page = 1, int pageSize = 20)
    {
        var url = $"api/streams/search?q={Uri.EscapeDataString(query)}&page={page}&pageSize={pageSize}";
        var response = await _httpClient.GetFromJsonAsync<List<Stream>>(url);
        return response ?? new List<Stream>();
    }

    // Categories
    public async Task<List<Category>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Category>>("api/categories");
        return response ?? new List<Category>();
    }

    public async Task<List<Stream>> GetStreamsByCategoryAsync(string categoryId, int page = 1, int pageSize = 20)
    {
        var url = $"api/categories/{categoryId}/streams?page={page}&pageSize={pageSize}";
        var response = await _httpClient.GetFromJsonAsync<List<Stream>>(url);
        return response ?? new List<Stream>();
    }

    // User
    public async Task<User?> GetCurrentUserAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<User>("api/users/me");
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<Stream>> GetFollowedStreamsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Stream>>("api/users/me/following/streams");
        return response ?? new List<Stream>();
    }

    public async Task<bool> FollowChannelAsync(string channelId)
    {
        var response = await _httpClient.PostAsync($"api/users/me/following/{channelId}", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UnfollowChannelAsync(string channelId)
    {
        var response = await _httpClient.DeleteAsync($"api/users/me/following/{channelId}");
        return response.IsSuccessStatusCode;
    }

    // Broadcast
    public async Task<string> GetStreamKeyAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<StreamKeyResponse>("api/users/me/streamkey");
        return response?.StreamKey ?? string.Empty;
    }

    public async Task<bool> UpdateStreamInfoAsync(string title, string categoryId)
    {
        var content = JsonContent.Create(new { title, categoryId });
        var response = await _httpClient.PutAsync("api/users/me/stream", content);
        return response.IsSuccessStatusCode;
    }

    private class StreamKeyResponse
    {
        public string StreamKey { get; set; } = string.Empty;
    }
}
