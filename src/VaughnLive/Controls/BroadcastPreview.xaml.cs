namespace VaughnLive.Controls;

public partial class BroadcastPreview : ContentView
{
    public static readonly BindableProperty IsPreviewingProperty =
        BindableProperty.Create(nameof(IsPreviewing), typeof(bool), typeof(BroadcastPreview), false);

    public bool IsPreviewing
    {
        get => (bool)GetValue(IsPreviewingProperty);
        set => SetValue(IsPreviewingProperty, value);
    }

    public BroadcastPreview()
    {
        InitializeComponent();
    }

    public async Task StartPreviewAsync()
    {
        // Platform-specific camera preview initialization
        // Will be implemented in platform handlers
        IsPreviewing = true;
        await Task.CompletedTask;
    }

    public async Task StopPreviewAsync()
    {
        IsPreviewing = false;
        await Task.CompletedTask;
    }

    public void SwitchCamera()
    {
        // Switch between front and back camera
        // Will be implemented in platform handlers
    }
}
