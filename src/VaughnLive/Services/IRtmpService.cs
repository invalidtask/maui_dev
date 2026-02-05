using VaughnLive.Models;

namespace VaughnLive.Services;

public interface IRtmpService
{
    Task<bool> StartBroadcastAsync(BroadcastSettings settings);
    Task StopBroadcastAsync();
    event EventHandler<BroadcastStats>? StatsUpdated;
    event EventHandler<string>? ErrorOccurred;
    bool IsStreaming { get; }
}
