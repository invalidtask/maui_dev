using VaughnLive.Models;

namespace VaughnLive.Services;

public interface IStreamingService
{
    Task<List<LiveStream>> GetRecommendedStreamsAsync();
    Task<List<LiveStream>> GetTrendingStreamsAsync();
    Task<StreamPlaybackInfo?> GetPlaybackInfoAsync(string streamId);
    Task ReportStreamViewAsync(string streamId);
}

public class StreamPlaybackInfo
{
    public string PlaybackUrl { get; set; } = string.Empty;
    public string ChatUrl { get; set; } = string.Empty;
    public string? BackupUrl { get; set; }
    public int RecommendedBitrate { get; set; }
}
