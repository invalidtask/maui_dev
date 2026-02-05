using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VaughnLive.Models;
using VaughnLive.Services;

namespace VaughnLive.ViewModels;

public partial class BroadcastViewModel : BaseViewModel
{
    private readonly IRtmpService _rtmpService;
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string _streamTitle = string.Empty;

    [ObservableProperty]
    private string _selectedCategory = string.Empty;

    [ObservableProperty]
    private bool _isStreaming;

    [ObservableProperty]
    private bool _isMuted;

    [ObservableProperty]
    private CameraPosition _cameraPosition = CameraPosition.Back;

    [ObservableProperty]
    private VideoResolution _resolution = VideoResolution.HD720;

    [ObservableProperty]
    private BroadcastStats? _currentStats;

    [ObservableProperty]
    private int _viewerCount;

    [ObservableProperty]
    private string _chatUrl = string.Empty;

    public BroadcastViewModel(IRtmpService rtmpService, IAuthService authService)
    {
        _rtmpService = rtmpService;
        _authService = authService;

        _rtmpService.StatsUpdated += OnStatsUpdated;
        _rtmpService.ErrorOccurred += OnErrorOccurred;

        Title = "Go Live";
    }

    [RelayCommand]
    private async Task GoLiveAsync()
    {
        if (string.IsNullOrWhiteSpace(StreamTitle))
        {
            await Shell.Current.DisplayAlert("Error", "Please enter a stream title", "OK");
            return;
        }

        IsBusy = true;

        try
        {
            var streamKey = await _authService.GetStreamKeyAsync();

            var settings = new BroadcastSettings
            {
                StreamKey = streamKey,
                RtmpUrl = "rtmp://live.vaughnsoft.net/live",
                Resolution = Resolution,
                FrameRate = 30,
                VideoBitrate = GetBitrateForResolution(Resolution),
                CameraPosition = CameraPosition,
                UseHardwareEncoding = true
            };

            var success = await _rtmpService.StartBroadcastAsync(settings);

            if (success)
            {
                IsStreaming = true;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to start stream: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EndStreamAsync()
    {
        var confirm = await Shell.Current.DisplayAlert(
            "End Stream",
            "Are you sure you want to end your stream?",
            "End Stream",
            "Cancel");

        if (confirm)
        {
            await _rtmpService.StopBroadcastAsync();
            IsStreaming = false;
        }
    }

    [RelayCommand]
    private void ToggleCamera()
    {
        CameraPosition = CameraPosition == CameraPosition.Front
            ? CameraPosition.Back
            : CameraPosition.Front;
    }

    [RelayCommand]
    private void ToggleMute()
    {
        IsMuted = !IsMuted;
    }

    private int GetBitrateForResolution(VideoResolution resolution) => resolution switch
    {
        VideoResolution.SD480 => 1500,
        VideoResolution.HD720 => 2500,
        VideoResolution.FHD1080 => 4500,
        _ => 2500
    };

    private void OnStatsUpdated(object? sender, BroadcastStats stats)
    {
        MainThread.BeginInvokeOnMainThread(() => CurrentStats = stats);
    }

    private void OnErrorOccurred(object? sender, string error)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Shell.Current.DisplayAlert("Broadcast Error", error, "OK");
        });
    }
}
