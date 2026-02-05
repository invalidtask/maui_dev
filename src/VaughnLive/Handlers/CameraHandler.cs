namespace VaughnLive.Handlers;

/// <summary>
/// Platform-specific camera handler for broadcast preview.
/// This handler manages camera initialization, preview rendering, and camera switching.
/// </summary>
public class CameraHandler
{
    private bool _isInitialized;
    private bool _isFrontCamera;

    public bool IsInitialized => _isInitialized;
    public bool IsFrontCamera => _isFrontCamera;

    public event EventHandler<byte[]>? FrameCaptured;
    public event EventHandler<string>? ErrorOccurred;

    public async Task InitializeAsync()
    {
        // Request camera permissions
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                ErrorOccurred?.Invoke(this, "Camera permission denied");
                return;
            }
        }

        // Platform-specific initialization will happen in platform implementations
        _isInitialized = true;
    }

    public async Task StartPreviewAsync()
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }

        // Platform-specific preview start
        await Task.CompletedTask;
    }

    public async Task StopPreviewAsync()
    {
        // Platform-specific preview stop
        await Task.CompletedTask;
    }

    public void SwitchCamera()
    {
        _isFrontCamera = !_isFrontCamera;
        // Platform-specific camera switch
    }

    public void Dispose()
    {
        _isInitialized = false;
    }
}
