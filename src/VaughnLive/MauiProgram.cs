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
        builder.Services.AddTransient<FollowingPage>();

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
