using LibVLCSharp.Shared;

namespace VaughnLive.Controls;

public partial class VideoPlayer : ContentView
{
    private LibVLC? _libVLC;
    private MediaPlayer? _mediaPlayer;
    private bool _isPlaying;

    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source), typeof(string), typeof(VideoPlayer),
            propertyChanged: OnSourceChanged);

    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public event EventHandler? FullscreenRequested;

    public VideoPlayer()
    {
        InitializeComponent();
    }

    private static async void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is VideoPlayer player && newValue is string url && !string.IsNullOrEmpty(url))
        {
            await player.InitializePlayerAsync(url);
        }
    }

    private async Task InitializePlayerAsync(string url)
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

            MainThread.BeginInvokeOnMainThread(() =>
            {
                VlcVideoView.MediaPlayer = _mediaPlayer;
            });

            using var media = new Media(_libVLC, new Uri(url));
            media.AddOption(":demux=avformat");
            media.AddOption(":avformat-format=flv");

            _mediaPlayer.Media = media;
            _mediaPlayer.Playing += OnMediaPlaying;
            _mediaPlayer.Play();
        });
    }

    private void OnMediaPlaying(object? sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            _isPlaying = true;
            PlayPauseButton.Source = "pause_icon.png";
        });
    }

    private void OnPlayPauseClicked(object? sender, EventArgs e)
    {
        if (_mediaPlayer == null) return;

        if (_isPlaying)
        {
            _mediaPlayer.Pause();
            PlayPauseButton.Source = "play_icon.png";
        }
        else
        {
            _mediaPlayer.Play();
            PlayPauseButton.Source = "pause_icon.png";
        }
        _isPlaying = !_isPlaying;
    }

    private void OnFullscreenClicked(object? sender, EventArgs e)
    {
        FullscreenRequested?.Invoke(this, EventArgs.Empty);
    }

    public void Stop()
    {
        _mediaPlayer?.Stop();
    }

    public void Dispose()
    {
        _mediaPlayer?.Stop();
        _mediaPlayer?.Dispose();
        _libVLC?.Dispose();
    }
}
