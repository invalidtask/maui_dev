namespace VaughnLive.Models;

public class Stream
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string StreamerName { get; set; } = string.Empty;
    public string StreamerAvatar { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int ViewerCount { get; set; }
    public DateTime StartedAt { get; set; }
    public string PlaybackUrl { get; set; } = string.Empty; // HTTP-FLV URL
    public string ChatUrl { get; set; } = string.Empty;
    public bool IsLive { get; set; }
    public List<string> Tags { get; set; } = new();
}
