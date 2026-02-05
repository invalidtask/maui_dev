using VaughnLive.ViewModels;

namespace VaughnLive.Views;

public partial class BroadcastPage : ContentPage
{
    private readonly BroadcastViewModel _viewModel;

    public BroadcastPage(BroadcastViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}
