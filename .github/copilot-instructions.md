# Vaughn Live - .NET MAUI Mobile Application

> **GitHub Copilot Instructions File**
> Place this file at `.github/copilot-instructions.md` in your repository for Copilot to use as context.

## Project Overview

Vaughn Live is a cross-platform mobile live streaming application built with .NET MAUI targeting Android and iOS. The app enables users to discover, watch, and broadcast live video content.

## Technology Stack

| Component | Technology |
|-----------|------------|
| Framework | .NET MAUI (.NET 8+) |
| IDE | Visual Studio 2026 |
| Language | C# 12+ |
| UI Framework | XAML with MVVM Pattern |
| Video Playback | LibVLCSharp (VLC SDK) - HTTP-FLV streams |
| Broadcasting | RTMP via FFmpeg/librtmp bindings |
| Chat Interface | WebView (Hybrid approach) |
| Target Platforms | Android 8.0+ (API 26), iOS 14.0+ |

## RTMP Server Configuration

```
Endpoint: rtmp://live.vaughnsoft.net/live
Video Codec: H.264 (AVC) with hardware encoding
Audio Codec: AAC-LC @ 128kbps stereo
Resolution Options: 720p (default), 480p, 1080p
Bitrate Range: 1500-6000 kbps (adaptive)
Frame Rate: 30fps (default), 60fps option
```

## Required NuGet Packages

```xml
<ItemGroup>
  <!-- VLC SDK for HTTP-FLV playback -->
  <PackageReference Include="LibVLCSharp" Version="3.*" />
  <PackageReference Include="LibVLCSharp.MAUI" Version="3.*" />
  
  <!-- MVVM Toolkit -->
  <PackageReference Include="CommunityToolkit.Mvvm" Version="8.*" />
  <PackageReference Include="CommunityToolkit.Maui" Version="7.*" />
  
  <!-- HTTP and JSON -->
  <PackageReference Include="Microsoft.Extensions.Http" Version="8.*" />
  <PackageReference Include="System.Text.Json" Version="8.*" />
  
  <!-- Image Loading -->
  <PackageReference Include="FFImageLoading.Maui" Version="1.*" />
  
  <!-- Audio Recording -->
  <PackageReference Include="Plugin.Maui.Audio" Version="2.*" />
  
  <!-- Logging -->
  <PackageReference Include="Serilog" Version="3.*" />
  <PackageReference Include="Serilog.Sinks.Console" Version="5.*" />
  <PackageReference Include="Serilog.Sinks.File" Version="5.*" />
</ItemGroup>
```

## Project Structure

```
VaughnLive/
├── VaughnLive.sln
├── src/
│   └── VaughnLive/
│       ├── VaughnLive.csproj
│       ├── App.xaml
│       ├── App.xaml.cs
│       ├── MauiProgram.cs
│       ├── AppShell.xaml
│       ├── AppShell.xaml.cs
│       │
│       ├── Models/
│       │   ├── Stream.cs
│       │   ├── User.cs
│       │   ├── Category.cs
│       │   ├── ChatMessage.cs
│       │   └── BroadcastSettings.cs
│       │
│       ├── ViewModels/
│       │   ├── BaseViewModel.cs
│       │   ├── HomeViewModel.cs
│       │   ├── SearchViewModel.cs
│       │   ├── StreamViewModel.cs
│       │   ├── BroadcastViewModel.cs
│       │   └── ProfileViewModel.cs
│       │
│       ├── Views/
│       │   ├── HomePage.xaml
│       │   ├── SearchPage.xaml
│       │   ├── StreamPage.xaml
│       │   ├── BroadcastPage.xaml
│       │   └── ProfilePage.xaml
│       │
│       ├── Services/
│       │   ├── IApiService.cs
│       │   ├── ApiService.cs
│       │   ├── IStreamingService.cs
│       │   ├── StreamingService.cs
│       │   ├── IRtmpService.cs
│       │   ├── RtmpService.cs
│       │   ├── IAuthService.cs
│       │   └── AuthService.cs
│       │
│       ├── Controls/
│       │   ├── StreamCard.xaml
│       │   ├── VideoPlayer.xaml
│       │   ├── ChatWebView.xaml
│       │   └── BroadcastPreview.xaml
│       │
│       ├── Handlers/
│       │   ├── CameraHandler.cs
│       │   └── RtmpHandler.cs
│       │
│       ├── Converters/
│       │   ├── ViewerCountConverter.cs
│       │   ├── DurationConverter.cs
│       │   └── BoolToVisibilityConverter.cs
│       │
│       ├── Resources/
│       │   ├── Styles/
│       │   │   ├── Colors.xaml
│       │   │   └── Styles.xaml
│       │   ├── Fonts/
│       │   └── Images/
│       │
│       └── Platforms/
│           ├── Android/
│           │   ├── MainActivity.cs
│           │   ├── MainApplication.cs
│           │   └── Services/
│           │       └── AndroidRtmpService.cs
│           └── iOS/
│               ├── AppDelegate.cs
│               ├── Program.cs
│               └── Services/
│                   └── iOSRtmpService.cs
│
└── tests/
    └── VaughnLive.Tests/
        ├── ViewModels/
        └── Services/
```

## Architecture Pattern: MVVM

### Base ViewModel Implementation

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace VaughnLive.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private bool _isRefreshing;

    public virtual Task InitializeAsync() => Task.CompletedTask;
}
```

### Service Registration in MauiProgram.cs

```csharp
using CommunityToolkit.Maui;
using LibVLCSharp.MAUI;
using Microsoft.Extensions.Logging;
using VaughnLive.Services;
using VaughnLive.ViewModels;
using VaughnLive.Views;

namespace VaughnLive;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseLibVLCSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                fonts.AddFont("Inter-Bold.ttf", "InterBold");
                fonts.AddFont("Inter-Medium.ttf", "InterMedium");
            });

        // Services
        builder.Services.AddSingleton<IApiService, ApiService>();
        builder.Services.AddSingleton<IStreamingService, StreamingService>();
        builder.Services.AddSingleton<IRtmpService, RtmpService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();

        // ViewModels
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<SearchViewModel>();
        builder.Services.AddTransient<StreamViewModel>();
        builder.Services.AddTransient<BroadcastViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();

        // Views
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<SearchPage>();
        builder.Services.AddTransient<StreamPage>();
        builder.Services.AddTransient<BroadcastPage>();
        builder.Services.AddTransient<ProfilePage>();

        // HTTP Client
        builder.Services.AddHttpClient("VaughnLiveApi", client =>
        {
            client.BaseAddress = new Uri("https://api.vaughnsoft.net/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
```

## Core Features Implementation

### 1. Stream Model

```csharp
namespace VaughnLive.Models;

public class Stream
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string StreamerName { get; set; } = string.Empty;
    public string StreamerAvatar { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int ViewerCount { get; set; }
    public DateTime StartedAt { get; set; }
    public string PlaybackUrl { get; set; } = string.Empty; // HTTP-FLV URL
    public string ChatUrl { get; set; } = string.Empty;
    public bool IsLive { get; set; }
    public List<string> Tags { get; set; } = new();
}
```

### 2. Stream Playback with VLC SDK (HTTP-FLV)

```csharp
using LibVLCSharp.Shared;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace VaughnLive.ViewModels;

public partial class StreamViewModel : BaseViewModel
{
    private LibVLC? _libVLC;
    private MediaPlayer? _mediaPlayer;

    [ObservableProperty]
    private Stream? _currentStream;

    [ObservableProperty]
    private bool _isPlaying;

    [ObservableProperty]
    private bool _isFullscreen;

    [ObservableProperty]
    private bool _isChatVisible = true;

    [ObservableProperty]
    private string _chatUrl = string.Empty;

    public MediaPlayer? MediaPlayer => _mediaPlayer;

    public async Task LoadStreamAsync(string streamId)
    {
        IsBusy = true;

        try
        {
            // Fetch stream details from API
            CurrentStream = await _apiService.GetStreamAsync(streamId);

            if (CurrentStream != null)
            {
                ChatUrl = CurrentStream.ChatUrl;
                await InitializePlayerAsync(CurrentStream.PlaybackUrl);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task InitializePlayerAsync(string httpFlvUrl)
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

    public void Dispose()
    {
        _mediaPlayer?.Stop();
        _mediaPlayer?.Dispose();
        _libVLC?.Dispose();
    }
}
```

### 3. RTMP Broadcasting Service

```csharp
namespace VaughnLive.Services;

public interface IRtmpService
{
    Task<bool> StartBroadcastAsync(BroadcastSettings settings);
    Task StopBroadcastAsync();
    event EventHandler<BroadcastStats>? StatsUpdated;
    event EventHandler<string>? ErrorOccurred;
    bool IsStreaming { get; }
}

public class BroadcastSettings
{
    public string StreamKey { get; set; } = string.Empty;
    public string RtmpUrl { get; set; } = "rtmp://live.vaughnsoft.net/live";
    public int VideoBitrate { get; set; } = 2500; // kbps
    public int AudioBitrate { get; set; } = 128; // kbps
    public VideoResolution Resolution { get; set; } = VideoResolution.HD720;
    public int FrameRate { get; set; } = 30;
    public bool UseHardwareEncoding { get; set; } = true;
    public CameraPosition CameraPosition { get; set; } = CameraPosition.Back;
}

public enum VideoResolution
{
    SD480,   // 854x480
    HD720,   // 1280x720
    FHD1080  // 1920x1080
}

public enum CameraPosition
{
    Front,
    Back
}

public class BroadcastStats
{
    public int Bitrate { get; set; }
    public int DroppedFrames { get; set; }
    public int Fps { get; set; }
    public TimeSpan Duration { get; set; }
    public ConnectionQuality Quality { get; set; }
}

public enum ConnectionQuality
{
    Excellent,
    Good,
    Fair,
    Poor
}
```

### 4. Broadcast ViewModel

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
```

### 5. WebView Chat Integration

```csharp
namespace VaughnLive.Controls;

public class ChatWebView : WebView
{
    public static readonly BindableProperty ChatUrlProperty =
        BindableProperty.Create(nameof(ChatUrl), typeof(string), typeof(ChatWebView),
            propertyChanged: OnChatUrlChanged);

    public string ChatUrl
    {
        get => (string)GetValue(ChatUrlProperty);
        set => SetValue(ChatUrlProperty, value);
    }

    private static void OnChatUrlChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ChatWebView webView && newValue is string url && !string.IsNullOrEmpty(url))
        {
            webView.Source = new UrlWebViewSource { Url = url };
        }
    }

    public ChatWebView()
    {
        Navigating += OnNavigating;
    }

    private void OnNavigating(object? sender, WebNavigatingEventArgs e)
    {
        // Handle JavaScript callbacks from chat
        if (e.Url.StartsWith("vaughnlive://"))
        {
            e.Cancel = true;
            HandleChatCallback(e.Url);
        }
    }

    private void HandleChatCallback(string url)
    {
        var uri = new Uri(url);

        switch (uri.Host)
        {
            case "emote-picker":
                // Open native emote picker
                break;
            case "notification":
                // Handle chat notification
                break;
        }
    }

    public async Task SendMessageAsync(string message)
    {
        var escapedMessage = message.Replace("'", "\\'");
        await EvaluateJavaScriptAsync($"sendChatMessage('{escapedMessage}')");
    }
}
```

## UI Theme Colors

### Colors.xaml

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<?xaml-comp compile="true" ?>
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">

    <!-- Dark Theme (Default) -->
    <Color x:Key="BackgroundDark">#0E0E10</Color>
    <Color x:Key="SurfaceDark">#18181B</Color>
    <Color x:Key="SurfaceElevatedDark">#1F1F23</Color>
    <Color x:Key="TextPrimaryDark">#EFEFF1</Color>
    <Color x:Key="TextSecondaryDark">#ADADB8</Color>

    <!-- Light Theme -->
    <Color x:Key="BackgroundLight">#F7F7F8</Color>
    <Color x:Key="SurfaceLight">#FFFFFF</Color>
    <Color x:Key="SurfaceElevatedLight">#FAFAFA</Color>
    <Color x:Key="TextPrimaryLight">#0E0E10</Color>
    <Color x:Key="TextSecondaryLight">#53535F</Color>

    <!-- Accent Colors -->
    <Color x:Key="PrimaryAccent">#9146FF</Color>
    <Color x:Key="PrimaryAccentHover">#772CE8</Color>
    <Color x:Key="LiveIndicator">#EB0400</Color>
    <Color x:Key="Success">#00C853</Color>
    <Color x:Key="Warning">#FFB300</Color>
    <Color x:Key="Error">#FF5252</Color>

    <!-- Gradients -->
    <LinearGradientBrush x:Key="PrimaryGradient" StartPoint="0,0" EndPoint="1,1">
        <GradientStop Color="#9146FF" Offset="0.0"/>
        <GradientStop Color="#772CE8" Offset="1.0"/>
    </LinearGradientBrush>

</ResourceDictionary>
```

## Stream Card Component

### StreamCard.xaml

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:models="clr-namespace:VaughnLive.Models"
             x:Class="VaughnLive.Controls.StreamCard"
             x:DataType="models:Stream">

    <Frame Padding="0"
           CornerRadius="8"
           BackgroundColor="{StaticResource SurfaceDark}"
           HasShadow="False">
        <Grid RowDefinitions="Auto,Auto">
            
            <!-- Thumbnail -->
            <Grid>
                <Image Source="{Binding ThumbnailUrl}"
                       Aspect="AspectFill"
                       HeightRequest="100"/>

                <!-- Live Badge -->
                <Frame IsVisible="{Binding IsLive}"
                       BackgroundColor="{StaticResource LiveIndicator}"
                       CornerRadius="4"
                       Padding="6,2"
                       Margin="8"
                       HorizontalOptions="Start"
                       VerticalOptions="Start">
                    <Label Text="LIVE"
                           TextColor="White"
                           FontSize="10"
                           FontAttributes="Bold"/>
                </Frame>

                <!-- Viewer Count -->
                <Frame BackgroundColor="#80000000"
                       CornerRadius="4"
                       Padding="6,2"
                       Margin="8"
                       HorizontalOptions="End"
                       VerticalOptions="End">
                    <HorizontalStackLayout Spacing="4">
                        <Image Source="viewer_icon.png"
                               HeightRequest="12"
                               WidthRequest="12"/>
                        <Label Text="{Binding ViewerCount, StringFormat='{0:N0}'}"
                               TextColor="White"
                               FontSize="11"/>
                    </HorizontalStackLayout>
                </Frame>
            </Grid>

            <!-- Stream Info -->
            <VerticalStackLayout Grid.Row="1" Padding="8" Spacing="4">
                <HorizontalStackLayout Spacing="8">
                    <Frame CornerRadius="16"
                           HeightRequest="32"
                           WidthRequest="32"
                           Padding="0"
                           IsClippedToBounds="True">
                        <Image Source="{Binding StreamerAvatar}"
                               Aspect="AspectFill"/>
                    </Frame>
                    <VerticalStackLayout>
                        <Label Text="{Binding Title}"
                               TextColor="{StaticResource TextPrimaryDark}"
                               FontSize="13"
                               LineBreakMode="TailTruncation"
                               MaxLines="1"/>
                        <Label Text="{Binding StreamerName}"
                               TextColor="{StaticResource TextSecondaryDark}"
                               FontSize="12"/>
                    </VerticalStackLayout>
                </HorizontalStackLayout>
                <Label Text="{Binding Category}"
                       TextColor="{StaticResource TextSecondaryDark}"
                       FontSize="11"/>
            </VerticalStackLayout>
        </Grid>
    </Frame>
</ContentView>
```

## API Service Interface

```csharp
namespace VaughnLive.Services;

public interface IApiService
{
    // Streams
    Task<List<Stream>> GetFeaturedStreamsAsync();
    Task<List<Stream>> GetLiveStreamsAsync(string? category = null, int page = 1, int pageSize = 20);
    Task<Stream?> GetStreamAsync(string streamId);
    Task<List<Stream>> SearchStreamsAsync(string query, int page = 1, int pageSize = 20);

    // Categories
    Task<List<Category>> GetCategoriesAsync();
    Task<List<Stream>> GetStreamsByCategoryAsync(string categoryId, int page = 1, int pageSize = 20);

    // User
    Task<User?> GetCurrentUserAsync();
    Task<List<Stream>> GetFollowedStreamsAsync();
    Task<bool> FollowChannelAsync(string channelId);
    Task<bool> UnfollowChannelAsync(string channelId);

    // Broadcast
    Task<string> GetStreamKeyAsync();
    Task<bool> UpdateStreamInfoAsync(string title, string categoryId);
}
```

## Platform-Specific Permissions

### Android - AndroidManifest.xml additions

```xml
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
<uses-permission android:name="android.permission.CAMERA" />
<uses-permission android:name="android.permission.RECORD_AUDIO" />
<uses-permission android:name="android.permission.MODIFY_AUDIO_SETTINGS" />
<uses-permission android:name="android.permission.FOREGROUND_SERVICE" />
<uses-permission android:name="android.permission.FOREGROUND_SERVICE_CAMERA" />
<uses-permission android:name="android.permission.FOREGROUND_SERVICE_MICROPHONE" />

<uses-feature android:name="android.hardware.camera" android:required="true" />
<uses-feature android:name="android.hardware.camera.autofocus" android:required="false" />
<uses-feature android:name="android.hardware.microphone" android:required="true" />
```

### iOS - Info.plist additions

```xml
<key>NSCameraUsageDescription</key>
<string>Vaughn Live needs camera access to broadcast live video</string>
<key>NSMicrophoneUsageDescription</key>
<string>Vaughn Live needs microphone access to broadcast audio</string>
<key>UIBackgroundModes</key>
<array>
    <string>audio</string>
    <string>voip</string>
</array>
```

## Navigation Structure (AppShell.xaml)

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
       xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
       xmlns:views="clr-namespace:VaughnLive.Views"
       x:Class="VaughnLive.AppShell"
       Shell.FlyoutBehavior="Disabled">

    <TabBar>
        <ShellContent Title="Home"
                      Icon="home_icon.png"
                      ContentTemplate="{DataTemplate views:HomePage}"/>

        <ShellContent Title="Search"
                      Icon="search_icon.png"
                      ContentTemplate="{DataTemplate views:SearchPage}"/>

        <ShellContent Title="Go Live"
                      Icon="broadcast_icon.png"
                      ContentTemplate="{DataTemplate views:BroadcastPage}"/>

        <ShellContent Title="Following"
                      Icon="following_icon.png"
                      ContentTemplate="{DataTemplate views:FollowingPage}"/>

        <ShellContent Title="Profile"
                      Icon="profile_icon.png"
                      ContentTemplate="{DataTemplate views:ProfilePage}"/>
    </TabBar>

    <!-- Route Registration -->
    <Shell.Routes>
        <ShellContent Route="stream" ContentTemplate="{DataTemplate views:StreamPage}"/>
    </Shell.Routes>

</Shell>
```

## Coding Conventions

- Use `partial` classes with `[ObservableProperty]` and `[RelayCommand]` attributes from CommunityToolkit.Mvvm
- Prefer `async/await` for all asynchronous operations
- Use `MainThread.BeginInvokeOnMainThread()` for UI updates from background threads
- Implement `IDisposable` for ViewModels that manage native resources
- Use dependency injection for all services
- Follow nullable reference types (`#nullable enable`)
- Use file-scoped namespaces
- Prefix private fields with underscore (`_fieldName`)
- Use expression-bodied members where appropriate

## Build Commands

```bash
# Restore packages
dotnet restore

# Build for Android
dotnet build -f net8.0-android

# Build for iOS
dotnet build -f net8.0-ios

# Run on Android emulator
dotnet build -t:Run -f net8.0-android

# Run on iOS simulator
dotnet build -t:Run -f net8.0-ios

# Publish for Android
dotnet publish -f net8.0-android -c Release

# Publish for iOS
dotnet publish -f net8.0-ios -c Release -p:ArchiveOnBuild=true
```
