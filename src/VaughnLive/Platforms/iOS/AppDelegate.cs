using Foundation;
using UIKit;

namespace VaughnLive;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // Configure audio session for streaming
        var audioSession = AVFoundation.AVAudioSession.SharedInstance();
        audioSession.SetCategory(AVFoundation.AVAudioSessionCategory.PlayAndRecord,
            AVFoundation.AVAudioSessionCategoryOptions.DefaultToSpeaker |
            AVFoundation.AVAudioSessionCategoryOptions.AllowBluetooth);
        audioSession.SetActive(true);

        return base.FinishedLaunching(application, launchOptions);
    }
}
