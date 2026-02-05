using VaughnLive.Models;

namespace VaughnLive.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(string username, string password);
    Task<bool> RegisterAsync(string username, string email, string password);
    Task LogoutAsync();
    Task<string> GetStreamKeyAsync();
    Task<bool> RefreshTokenAsync();
    bool IsAuthenticated { get; }
    User? CurrentUser { get; }
}
