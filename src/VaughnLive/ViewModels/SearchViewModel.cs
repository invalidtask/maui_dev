using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VaughnLive.Models;
using VaughnLive.Services;

namespace VaughnLive.ViewModels;

public partial class SearchViewModel : BaseViewModel
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private List<Stream> _searchResults = new();

    [ObservableProperty]
    private List<Category> _categories = new();

    [ObservableProperty]
    private bool _hasSearched;

    public SearchViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Title = "Search";
    }

    public override async Task InitializeAsync()
    {
        await LoadCategoriesAsync();
    }

    private async Task LoadCategoriesAsync()
    {
        try
        {
            Categories = await _apiService.GetCategoriesAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery)) return;

        IsBusy = true;
        HasSearched = true;

        try
        {
            SearchResults = await _apiService.SearchStreamsAsync(SearchQuery);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Search failed: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToStreamAsync(Stream stream)
    {
        if (stream == null) return;

        await Shell.Current.GoToAsync($"stream?id={stream.Id}");
    }

    [RelayCommand]
    private void ClearSearch()
    {
        SearchQuery = string.Empty;
        SearchResults = new();
        HasSearched = false;
    }
}
