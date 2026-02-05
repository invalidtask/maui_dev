namespace VaughnLive.Models;

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
