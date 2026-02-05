namespace VaughnLive.Models;

public class Category
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int ViewerCount { get; set; }
    public int StreamCount { get; set; }
}
