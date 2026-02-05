using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VaughnLive.Models;
using VaughnLive.Services;

namespace VaughnLive.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private List<LiveStream> _featuredStreams = new();

    [ObservableProperty]
    private List<LiveStream> _liveStreams = new();

    [ObservableProperty]
    private List<Category> _categories = new();

    public HomeViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Title = "Home";
    }

    public override async Task InitializeAsync()
    {
        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsBusy) return;

        IsBusy = true;

        try
        {
            var featuredTask = _apiService.GetFeaturedStreamsAsync();
            var liveTask = _apiService.GetLiveStreamsAsync();
            var categoriesTask = _apiService.GetCategoriesAsync();

            await Task.WhenAll(featuredTask, liveTask, categoriesTask);

            FeaturedStreams = await featuredTask;
            LiveStreams = await liveTask;
            Categories = await categoriesTask;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToStreamAsync(LiveStream? stream)
    {
        if (stream == null) return;

        await Shell.Current.GoToAsync($"stream?id={stream.Id}");
    }

    [RelayCommand]
    private async Task NavigateToCategoryAsync(Category category)
    {
        if (category == null) return;

        await Shell.Current.GoToAsync($"category?id={category.Id}");
    }
}
