using System.Net.Http.Json;
using VaughnLive.Models;

namespace VaughnLive.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private string? _accessToken;
    private User? _currentUser;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_accessToken);
    public User? CurrentUser => _currentUser;

    public AuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("VaughnLiveApi");
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { username, password });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null)
                {
                    _accessToken = result.AccessToken;
                    _currentUser = result.User;
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);
                    
                    // Store token securely
                    await SecureStorage.SetAsync("access_token", _accessToken);
                    
                    return true;
                }
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RegisterAsync(string username, string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register",
                new { username, email, password });

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        _accessToken = null;
        _currentUser = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
        
        SecureStorage.Remove("access_token");
        
        await Task.CompletedTask;
    }

    public async Task<string> GetStreamKeyAsync()
    {
        if (!IsAuthenticated)
        {
            throw new InvalidOperationException("User is not authenticated");
        }

        var response = await _httpClient.GetFromJsonAsync<StreamKeyResponse>("api/users/me/streamkey");
        return response?.StreamKey ?? string.Empty;
    }

    public async Task<bool> RefreshTokenAsync()
    {
        try
        {
            var storedToken = await SecureStorage.GetAsync("access_token");
            if (string.IsNullOrEmpty(storedToken)) return false;

            var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", new { token = storedToken });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null)
                {
                    _accessToken = result.AccessToken;
                    _currentUser = result.User;
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);
                    
                    await SecureStorage.SetAsync("access_token", _accessToken);
                    
                    return true;
                }
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    private class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public User? User { get; set; }
    }

    private class StreamKeyResponse
    {
        public string StreamKey { get; set; } = string.Empty;
    }
}
