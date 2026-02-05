using LibVLCSharp.Shared;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VaughnLive.Services;

namespace VaughnLive.ViewModels;

public partial class StreamViewModel : BaseViewModel, IDisposable
{
    private readonly IApiService _apiService;
    private LibVLC? _libVLC;
    private MediaPlayer? _mediaPlayer;

    [ObservableProperty]
    private Models.Stream? _currentStream;

    [ObservableProperty]
    private bool _isPlaying;

    [ObservableProperty]
    private bool _isFullscreen;

    [ObservableProperty]
    private bool _isChatVisible = true;

    [ObservableProperty]
    private string _chatUrl = string.Empty;

    public MediaPlayer? MediaPlayer => _mediaPlayer;

    public StreamViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Title = "Stream";
    }

    public async Task LoadStreamAsync(string streamId)
    {
        IsBusy = true;

        try
        {
            // Fetch stream details from API
            CurrentStream = await _apiService.GetStreamAsync(streamId);

            if (CurrentStream != null)
            {
                Title = CurrentStream.Title;
                ChatUrl = CurrentStream.ChatUrl;
                await InitializePlayerAsync(CurrentStream.PlaybackUrl);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load stream: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task InitializePlayerAsync(string httpFlvUrl)
    {
        await Task.Run(() =>
        {
            Core.Initialize();

            _libVLC = new LibVLC(
                "--network-caching=1000",
                "--live-caching=1000",
                "--file-caching=1000"
            );

            _mediaPlayer = new MediaPlayer(_libVLC);

            using var media = new Media(_libVLC, new Uri(httpFlvUrl));

            // Configure for low-latency HTTP-FLV
            media.AddOption(":demux=avformat");
            media.AddOption(":avformat-format=flv");

            _mediaPlayer.Media = media;
            _mediaPlayer.Play();
        });

        IsPlaying = true;
    }

    [RelayCommand]
    private void TogglePlayPause()
    {
        if (_mediaPlayer == null) return;

        if (IsPlaying)
        {
            _mediaPlayer.Pause();
        }
        else
        {
            _mediaPlayer.Play();
        }
        IsPlaying = !IsPlaying;
    }

    [RelayCommand]
    private void ToggleFullscreen()
    {
        IsFullscreen = !IsFullscreen;
        IsChatVisible = !IsFullscreen;
    }

    [RelayCommand]
    private void ToggleChat()
    {
        IsChatVisible = !IsChatVisible;
    }

    [RelayCommand]
    private async Task FollowChannelAsync()
    {
        if (CurrentStream == null) return;

        try
        {
            await _apiService.FollowChannelAsync(CurrentStream.Id);
            await Shell.Current.DisplayAlert("Success", $"You are now following {CurrentStream.StreamerName}", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to follow: {ex.Message}", "OK");
        }
    }

    public void Dispose()
    {
        _mediaPlayer?.Stop();
        _mediaPlayer?.Dispose();
        _libVLC?.Dispose();
    }
}
