using VaughnLive.Services;

namespace VaughnLive.Views;

public partial class LoginPage : ContentPage
{
    private readonly IAuthService _authService;

    public LoginPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        var username = UsernameEntry.Text;
        var password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Please enter username and password", "OK");
            return;
        }

        SignInButton.IsEnabled = false;

        try
        {
            var success = await _authService.LoginAsync(username, password);

            if (success)
            {
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Error", "Invalid username or password", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
        }
        finally
        {
            SignInButton.IsEnabled = true;
        }
    }

    private async void OnSignUpTapped(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("register");
    }
}
