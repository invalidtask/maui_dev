namespace VaughnLive.Models;

public class ChatMessage
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? BadgeUrl { get; set; }
    public string? Color { get; set; }
}
