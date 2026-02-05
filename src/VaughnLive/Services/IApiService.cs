using VaughnLive.Models;

namespace VaughnLive.Services;

public interface IApiService
{
    // Streams
    Task<List<Stream>> GetFeaturedStreamsAsync();
    Task<List<Stream>> GetLiveStreamsAsync(string? category = null, int page = 1, int pageSize = 20);
    Task<Stream?> GetStreamAsync(string streamId);
    Task<List<Stream>> SearchStreamsAsync(string query, int page = 1, int pageSize = 20);

    // Categories
    Task<List<Category>> GetCategoriesAsync();
    Task<List<Stream>> GetStreamsByCategoryAsync(string categoryId, int page = 1, int pageSize = 20);

    // User
    Task<User?> GetCurrentUserAsync();
    Task<List<Stream>> GetFollowedStreamsAsync();
    Task<bool> FollowChannelAsync(string channelId);
    Task<bool> UnfollowChannelAsync(string channelId);

    // Broadcast
    Task<string> GetStreamKeyAsync();
    Task<bool> UpdateStreamInfoAsync(string title, string categoryId);
}
