using VaughnLive.Models;

namespace VaughnLive.Services;

public class RtmpService : IRtmpService
{
    private bool _isStreaming;
    private CancellationTokenSource? _streamingCts;

    public bool IsStreaming => _isStreaming;

    public event EventHandler<BroadcastStats>? StatsUpdated;
    public event EventHandler<string>? ErrorOccurred;

    public async Task<bool> StartBroadcastAsync(BroadcastSettings settings)
    {
        if (_isStreaming)
        {
            ErrorOccurred?.Invoke(this, "Already streaming");
            return false;
        }

        try
        {
            // Platform-specific implementation will handle the actual RTMP streaming
            // This is a base implementation that can be overridden

            _streamingCts = new CancellationTokenSource();
            _isStreaming = true;

            // Start stats monitoring
            _ = MonitorStatsAsync(_streamingCts.Token);

            return true;
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex.Message);
            return false;
        }
    }

    public async Task StopBroadcastAsync()
    {
        if (!_isStreaming) return;

        _streamingCts?.Cancel();
        _isStreaming = false;

        await Task.CompletedTask;
    }

    private async Task MonitorStatsAsync(CancellationToken cancellationToken)
    {
        var startTime = DateTime.UtcNow;

        while (!cancellationToken.IsCancellationRequested && _isStreaming)
        {
            var stats = new BroadcastStats
            {
                Duration = DateTime.UtcNow - startTime,
                Bitrate = 2500,
                Fps = 30,
                DroppedFrames = 0,
                Quality = ConnectionQuality.Excellent
            };

            StatsUpdated?.Invoke(this, stats);

            await Task.Delay(1000, cancellationToken);
        }
    }
}
