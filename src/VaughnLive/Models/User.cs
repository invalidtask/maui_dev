namespace VaughnLive.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsVerified { get; set; }
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
    public string StreamKey { get; set; } = string.Empty;
}
