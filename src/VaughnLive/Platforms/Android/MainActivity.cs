using Android.App;
using Android.Content.PM;
using Android.OS;

namespace VaughnLive;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density,
    LaunchMode = LaunchMode.SingleTop,
    ScreenOrientation = ScreenOrientation.Portrait)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Keep screen on while streaming
        Window?.AddFlags(Android.Views.WindowManagerFlags.KeepScreenOn);
    }

    protected override void OnResume()
    {
        base.OnResume();
    }

    protected override void OnPause()
    {
        base.OnPause();
    }
}
