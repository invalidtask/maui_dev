using VaughnLive.Models;

namespace VaughnLive.Services;

public interface IApiService
{
    // Streams
    Task<List<LiveStream>> GetFeaturedStreamsAsync();
    Task<List<LiveStream>> GetLiveStreamsAsync(string? category = null, int page = 1, int pageSize = 20);
    Task<LiveStream?> GetStreamAsync(string streamId);
    Task<List<LiveStream>> SearchStreamsAsync(string query, int page = 1, int pageSize = 20);

    // Categories
    Task<List<Category>> GetCategoriesAsync();
    Task<List<LiveStream>> GetStreamsByCategoryAsync(string categoryId, int page = 1, int pageSize = 20);

    // User
    Task<User?> GetCurrentUserAsync();
    Task<List<LiveStream>> GetFollowedStreamsAsync();
    Task<bool> FollowChannelAsync(string channelId);
    Task<bool> UnfollowChannelAsync(string channelId);

    // Broadcast
    Task<string> GetStreamKeyAsync();
    Task<bool> UpdateStreamInfoAsync(string title, string categoryId);
}
