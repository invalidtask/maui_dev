using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VaughnLive.Models;
using VaughnLive.Services;

namespace VaughnLive.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IAuthService _authService;

    [ObservableProperty]
    private User? _currentUser;

    [ObservableProperty]
    private List<Stream> _followedStreams = new();

    [ObservableProperty]
    private bool _isLoggedIn;

    public ProfileViewModel(IApiService apiService, IAuthService authService)
    {
        _apiService = apiService;
        _authService = authService;
        Title = "Profile";
    }

    public override async Task InitializeAsync()
    {
        await LoadProfileAsync();
    }

    [RelayCommand]
    private async Task LoadProfileAsync()
    {
        IsBusy = true;

        try
        {
            CurrentUser = await _apiService.GetCurrentUserAsync();
            IsLoggedIn = CurrentUser != null;

            if (IsLoggedIn)
            {
                FollowedStreams = await _apiService.GetFollowedStreamsAsync();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load profile: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        // Navigate to login page or show login modal
        await Shell.Current.GoToAsync("login");
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        var confirm = await Shell.Current.DisplayAlert(
            "Logout",
            "Are you sure you want to logout?",
            "Logout",
            "Cancel");

        if (confirm)
        {
            await _authService.LogoutAsync();
            CurrentUser = null;
            IsLoggedIn = false;
            FollowedStreams = new();
        }
    }

    [RelayCommand]
    private async Task NavigateToStreamAsync(Stream stream)
    {
        if (stream == null) return;

        await Shell.Current.GoToAsync($"stream?id={stream.Id}");
    }

    [RelayCommand]
    private async Task EditProfileAsync()
    {
        await Shell.Current.GoToAsync("editprofile");
    }
}
