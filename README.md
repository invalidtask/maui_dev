# Vaughn Live

A cross-platform mobile live streaming application built with .NET MAUI targeting Android and iOS.

## Overview

Vaughn Live enables users to discover, watch, and broadcast live video content. The app features:

- **Live Stream Discovery**: Browse featured and live streams by category
- **Stream Playback**: Watch HTTP-FLV streams with low-latency VLC SDK integration
- **Live Broadcasting**: Go live with RTMP streaming to Vaughn servers
- **Chat Integration**: Hybrid WebView-based chat alongside video
- **User Profiles**: Follow channels and manage your account

## Technology Stack

| Component | Technology |
|-----------|------------|
| Framework | .NET MAUI (.NET 8+) |
| Language | C# 12+ |
| UI Framework | XAML with MVVM Pattern |
| Video Playback | LibVLCSharp (VLC SDK) |
| Broadcasting | RTMP via FFmpeg/librtmp |
| Chat Interface | WebView (Hybrid approach) |
| Target Platforms | Android 8.0+ (API 26), iOS 14.0+ |

## Project Structure

```
VaughnLive/
├── VaughnLive.sln
├── src/VaughnLive/           # Main MAUI app
│   ├── Models/               # Data models
│   ├── ViewModels/           # MVVM view models
│   ├── Views/                # XAML pages
│   ├── Services/             # Business logic services
│   ├── Controls/             # Custom UI controls
│   ├── Handlers/             # Platform handlers
│   ├── Converters/           # Value converters
│   ├── Resources/            # Styles, colors, images
│   └── Platforms/            # Platform-specific code
└── tests/VaughnLive.Tests/   # Unit tests
```

## Getting Started

### Prerequisites

- .NET 8 SDK or later
- Visual Studio 2022+ with MAUI workload
- Android SDK (API 26+) for Android development
- Xcode 14+ for iOS development (macOS only)

### Build Commands

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

# Run tests
dotnet test
```

### Publishing

```bash
# Publish for Android
dotnet publish -f net8.0-android -c Release

# Publish for iOS
dotnet publish -f net8.0-ios -c Release -p:ArchiveOnBuild=true
```

## Architecture

The app follows the **MVVM (Model-View-ViewModel)** pattern using CommunityToolkit.Mvvm:

- **Models**: Plain C# objects representing data entities
- **ViewModels**: Handle UI logic with `[ObservableProperty]` and `[RelayCommand]` attributes
- **Views**: XAML pages with data binding to ViewModels
- **Services**: Injectable services for API, streaming, and authentication

All services and ViewModels are registered via dependency injection in `MauiProgram.cs`.

## Key Features

### Stream Playback
Uses LibVLCSharp for low-latency HTTP-FLV stream playback with configurable caching options.

### RTMP Broadcasting
Platform-specific implementations for camera capture and H.264/AAC encoding with RTMP transmission.

### Theming
Dark theme by default with consistent color palette defined in `Resources/Styles/Colors.xaml`.

## License

Proprietary - Vaughn Software