using System.Globalization;

namespace VaughnLive.Converters;

/// <summary>
/// Converts a TimeSpan or DateTime to a human-readable duration string.
/// Examples: "2h 30m", "45m", "Live for 1h 15m"
/// </summary>
public class DurationConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        TimeSpan duration;

        if (value is TimeSpan ts)
        {
            duration = ts;
        }
        else if (value is DateTime startTime)
        {
            duration = DateTime.UtcNow - startTime;
        }
        else
        {
            return string.Empty;
        }

        if (duration.TotalHours >= 1)
        {
            return $"{(int)duration.TotalHours}h {duration.Minutes}m";
        }
        else if (duration.TotalMinutes >= 1)
        {
            return $"{(int)duration.TotalMinutes}m";
        }
        else
        {
            return $"{duration.Seconds}s";
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
