using VaughnLive.Services;

namespace VaughnLive.Views;

[QueryProperty(nameof(CategoryId), "id")]
public partial class CategoryPage : ContentPage
{
    private readonly IApiService _apiService;

    public string CategoryId { get; set; } = string.Empty;

    public CategoryPage(IApiService apiService)
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
            var streams = await _apiService.GetStreamsByCategoryAsync(CategoryId);
            StreamsCollection.ItemsSource = streams;

            // Update header
            if (streams.Any())
            {
                CategoryNameLabel.Text = streams.First().Category;
                CategoryViewersLabel.Text = $"{streams.Sum(s => s.ViewerCount):N0} viewers";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load category: {ex.Message}", "OK");
        }
        finally
        {
            RefreshView.IsRefreshing = false;
        }
    }
}
