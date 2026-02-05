using VaughnLive.Services;
using VaughnLive.Models;

namespace VaughnLive.Views;

public partial class FollowingPage : ContentPage
{
    private readonly IApiService _apiService;

    public FollowingPage(IApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;

        RefreshView.Command = new Command(async () => await LoadDataAsync());
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            var streams = await _apiService.GetFollowedStreamsAsync();
            FollowedStreamsCollection.ItemsSource = streams;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load followed streams: {ex.Message}", "OK");
        }
        finally
        {
            RefreshView.IsRefreshing = false;
        }
    }
}
