using VaughnLive.ViewModels;

namespace VaughnLive.Views;

[QueryProperty(nameof(StreamId), "id")]
public partial class StreamPage : ContentPage
{
    private readonly StreamViewModel _viewModel;

    public string StreamId { get; set; } = string.Empty;

    public StreamPage(StreamViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (!string.IsNullOrEmpty(StreamId))
        {
            await _viewModel.LoadStreamAsync(StreamId);
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.Dispose();
    }
}
